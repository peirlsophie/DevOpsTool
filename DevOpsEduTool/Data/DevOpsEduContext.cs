using Microsoft.EntityFrameworkCore;
using DevOpsEduTool.Models;

namespace DevOpsEduTool.Data
{
    public class DevOpsEduContext : DbContext
    {
        public DevOpsEduContext(DbContextOptions<DevOpsEduContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<UserProgress> UserProgress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optionale Seed-Daten
            modelBuilder.Entity<Module>().HasData(
                new Module { Id = 1, Name = "DevOps Fundamentals", ControllerAction = "DevOpsBasics", Description = "Intro to DevOps & CI/CD" },
                new Module { Id = 2, Name = "Docker & Container", ControllerAction = "Docker", Description = "Containerization basics" },
                new Module { Id = 3, Name = "CI/CD Pipelines", ControllerAction = "CICD", Description = "Build & deploy pipelines" }
            );
        }
    }
}