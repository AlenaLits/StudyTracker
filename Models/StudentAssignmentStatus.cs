using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StudyTracker.Models
{
    public class StudentAssignmentStatus
    {
        [Key]
        public int Id { get; set; }

        // Внешний ключ на студента
        public string StudentId { get; set; }

        // Навигационное свойство на студента
        public IdentityUser Student { get; set; }

        // Внешний ключ на задание
        public int AssignmentId { get; set; }

        // Навигационное свойство на задание
        public Assignment Assignment { get; set; }

        // Статус для конкретного студента и задания
        [Required]
        public string Status { get; set; } = "Не начато";
    }
}
