using System.Collections.Generic;

namespace StudyTracker.ViewModels
{
    public class CoursesWithStudentsVm
    {
        public List<CourseWithStudentsVm> Courses { get; set; }

        public CoursesWithStudentsVm()
        {
            Courses = new List<CourseWithStudentsVm>();
        }
    }
}
