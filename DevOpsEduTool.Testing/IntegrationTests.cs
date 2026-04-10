using DevOpsEduTool.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;


namespace DevOpsEduTool.Testing
{
    [TestFixture]
    public class IntegrationTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                // Set the environment to Testing
                builder.UseEnvironment("Testing");

                builder.ConfigureServices(services =>
                {
                    // 1. Remove the production database configuration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<DevOpsEduContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // 2. Add an In-Memory Database for clean, isolated testing
                    services.AddDbContext<DevOpsEduContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });

                    // 3. Build a service provider to ensure the DB is seeded
                    var sp = services.BuildServiceProvider();
                    using (var scope = sp.CreateScope())
                    {
                        var db = scope.ServiceProvider.GetRequiredService<DevOpsEduContext>();
                        db.Database.EnsureCreated(); // This triggers your Module seeding!
                    }
                    services.ConfigureApplicationCookie(options => {
                        options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.None;
                    });
                });
            });

            // Create a client that handles cookies automatically
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client?.Dispose();
            _factory?.Dispose();
        }
        [Test]
        public async Task FullUserJourney_ManualFlow_Works()
        {
            //Register
            var regModel = new Dictionary<string, string>
            {
                { "Username", "TestStudent" },
                { "Email", "student@devops.edu" },
                { "Password", "Pass123!" }
            };
            await _client.PostAsync("/Account/Register", new FormUrlEncodedContent(regModel));

            //Login
            var loginModel = new Dictionary<string, string>
            {
                { "username", "TestStudent" },
                { "password", "Pass123!" }
            };
            var loginResponse = await _client.PostAsync("/Account/Login", new FormUrlEncodedContent(loginModel));

            //Verify 
            var dashboardResponse = await _client.GetAsync("/Home/Index");
            var html = await dashboardResponse.Content.ReadAsStringAsync();

            //Check if Login is Gone
            Assert.That(html, Does.Contain("TestStudent"), "Username should be visible after manual login.");
            Assert.That(loginResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

    }
}
