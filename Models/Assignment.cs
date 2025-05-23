using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyTracker.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        // Внешний ключ на курс
        public int CourseId { get; set; }

        // Навигационное свойство к курсу
        public Course Course { get; set; }

        [Required(ErrorMessage = "Введите название задания")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; } = "Не начато";

        public DateTime? Deadline { get; set; }

        // Навигационные свойства на статусы заданий у студентов
        public ICollection<StudentAssignmentStatus> StudentAssignmentStatuses { get; set; } = new List<StudentAssignmentStatus>();
    }
}
