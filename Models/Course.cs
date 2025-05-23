using System.ComponentModel.DataAnnotations;
namespace StudyTracker.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название курса")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Введите имя преподавателя")]
        public string ProfessorName { get; set; }
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();

        public ICollection<StudentCourseAssignment> StudentCourseAssignments { get; set; } = new List<StudentCourseAssignment>();
        public Course() { }
        public Course(int id, string name, string? description, string professorName)
        {
            Id = id;
            Name = name;
            Description = description;
            ProfessorName = professorName;
        }
    }
}
