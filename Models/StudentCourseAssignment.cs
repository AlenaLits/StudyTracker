using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StudyTracker.Models
{
    public class StudentCourseAssignment
    {
        [Key]
        public int Id { get; set; }

        // Внешний ключ на студента (IdentityUser)
        public string StudentId { get; set; }

        // Навигационное свойство на студента
        public IdentityUser Student { get; set; }

        // Внешний ключ на курс
        public int CourseId { get; set; }

        // Навигационное свойство на курс
        public Course Course { get; set; }
    }
}
