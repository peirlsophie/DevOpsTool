using System.Diagnostics;
using DevOpsEduTool.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsEduTool.Controllers
{
    public class ModulesController : Controller
    {
        private readonly ILogger<ModulesController> _logger;

        public ModulesController(ILogger<ModulesController> logger)
        {
            _logger = logger;
        }

        public IActionResult DevOpsBasics()
        {
            return View();
        }

        public IActionResult CICD()
        {
            return View();
        }

        public IActionResult Docker()
        {
            return View();
        }
    }
}
