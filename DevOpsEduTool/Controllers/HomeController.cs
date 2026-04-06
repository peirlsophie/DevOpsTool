using System.Diagnostics;
using DevOpsEduTool.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsEduTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
