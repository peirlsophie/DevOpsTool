using System.Diagnostics;
using DevOpsEduTool.Models;
using Microsoft.AspNetCore.Mvc;
using DevOpsEduTool.Data;
using Microsoft.EntityFrameworkCore;

namespace DevOpsEduTool.Controllers
{
    public class ModulesController : Controller
    {
        private readonly DevOpsEduContext _context;

        public ModulesController(DevOpsEduContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> DevOpsBasics()
        {
            var module = await _context.Modules.FirstOrDefaultAsync(m => m.Id == 1);
            return View(module);
        }

        public async Task<IActionResult> Docker()
        {
            var module = await _context.Modules.FirstOrDefaultAsync(m => m.Id == 2);
            return View(module);
        }

        public async Task<IActionResult> CICD()
        {
            var module = await _context.Modules.FirstOrDefaultAsync(m => m.Id == 3);
            return View(module);
        }
        [HttpPost]
        public async Task<IActionResult> SaveProgress(int moduleId, string status = "completed")
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim);

            var progress = await _context.UserProgress
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ModuleId == moduleId);

            if (progress == null)
            {
                _context.UserProgress.Add(new UserProgress
                {
                    UserId = userId,
                    ModuleId = moduleId,
                    Status = status
                });
            }
            else
            {
                progress.Status = status;
            }

            await _context.SaveChangesAsync(); 
            return Ok();
        }
        [HttpGet]
        public async Task<JsonResult> GetUserProgress()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;

            if (userIdClaim == null)
            {
                return Json(new List<object>()); 
            }

            int userId = int.Parse(userIdClaim);

            var progressList = await _context.UserProgress
                .Where(p => p.UserId == userId)
                .Select(p => new {
                    moduleId = p.ModuleId,
                    status = p.Status
                })
                .ToListAsync();

            return Json(progressList);
        }

        [HttpPost]
        public async Task<IActionResult> ClearProgress()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null) return Unauthorized();
            int userId = int.Parse(userIdClaim);

            var userProgress = _context.UserProgress.Where(p => p.UserId == userId);
            _context.UserProgress.RemoveRange(userProgress);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
