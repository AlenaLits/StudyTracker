using StudyTracker.Models;
using StudyTracker.Services;
using StudyTracker.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTracker.ViewModelBuilders
{
    public class CoursesVmBuilder
    {
        private readonly CourseService _courseService;

        public CoursesVmBuilder(CourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<CoursesVm> GetCoursesVmAsync()
        {
            List<Course> courses = await _courseService.GetCoursesAsync();
            return new CoursesVm(courses);
        }
    }
}
