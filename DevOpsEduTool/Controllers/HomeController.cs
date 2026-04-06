using System.Diagnostics;
using DevOpsEduTool.Models;
using Microsoft.AspNetCore.Mvc;
using DevOpsEduTool.Data;
using Microsoft.EntityFrameworkCore;

namespace DevOpsEduTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DevOpsEduContext _context;


        public HomeController(ILogger<HomeController> logger, DevOpsEduContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var modules = _context.Modules.OrderBy(m => m.Id).ToList();
            return View(modules);
        }
    }
}
