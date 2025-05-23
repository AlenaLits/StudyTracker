using StudyTracker.Models;
namespace StudyTracker.ViewModels
{
    public class CoursesVm
    {
        public List<Course> Courses { get; set; }

        public CoursesVm(List<Course> courses)
        {
            Courses = courses;
        }
    }
}
