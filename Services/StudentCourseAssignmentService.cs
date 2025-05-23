using StudyTracker.Models;
using StudyTracker.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTracker.Services
{
    public class StudentCourseAssignmentService
    {
        private readonly StudentCourseAssignmentRepository _repository;

        public StudentCourseAssignmentService(StudentCourseAssignmentRepository repository)
        {
            _repository = repository;
        }

        public Task<List<StudentCourseAssignment>> GetAssignmentsByStudentAsync(string studentId)
        {
            return _repository.GetAssignmentsByStudentAsync(studentId);
        }

        public async Task AssignCourseToStudentAsync(string studentId, int courseId)
        {
            if (!await _repository.ExistsAsync(studentId, courseId))
            {
                await _repository.AddAssignmentAsync(new StudentCourseAssignment
                {
                    StudentId = studentId,
                    CourseId = courseId
                });
            }
        }

        public async Task RemoveAssignmentAsync(string studentId, int courseId)
        {
            var assignments = await _repository.GetAssignmentsAsync();
            var assignment = assignments.Find(a => a.StudentId == studentId && a.CourseId == courseId);
            if (assignment != null)
            {
                await _repository.RemoveAssignmentAsync(assignment);
            }
        }

        public Task<List<StudentCourseAssignment>> GetAssignmentsAsync()
        {
            return _repository.GetAssignmentsAsync();
        }
    }
}
