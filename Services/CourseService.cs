using StudyTracker.Models;
using StudyTracker.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTracker.Services
{
    public class CourseService
    {
        private readonly CourseRepository _repository;

        public CourseService(CourseRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Course>> GetCoursesAsync()
        {
            return _repository.GetCoursesAsync();
        }

        public Task<Course?> GetCourseByIdAsync(int id)
        {
            return _repository.GetCourseByIdAsync(id);
        }

        public Task AddCourseAsync(Course course)
        {
            return _repository.AddCourseAsync(course);
        }

        public Task UpdateCourseAsync(Course course)
        {
            return _repository.UpdateCourseAsync(course);
        }

        public Task RemoveCourseAsync(int id)
        {
            return _repository.RemoveCourseAsync(id);
        }
    }
}
