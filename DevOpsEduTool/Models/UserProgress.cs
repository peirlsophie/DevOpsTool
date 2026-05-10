using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DevOpsEduTool.Models
{
    public class UserProgress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public User User { get; set; } = null!;

        public int ModuleId { get; set; }
        [Required]
        public Module Module { get; set; } = null!;
        [Required]
        public string Status { get; set; } = "not started";// in-progress, completed
    }
}