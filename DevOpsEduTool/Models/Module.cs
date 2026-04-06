using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevOpsEduTool.Models
{
    public class Module
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;

        public string ControllerAction { get; set; } = string.Empty;

        public bool IsLocked { get; set; } = true;

        public List<UserProgress> Progresses { get; set; } = new List<UserProgress>();
    }
}