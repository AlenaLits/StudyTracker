using StudyTracker.Models;
using StudyTracker.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTracker.Services
{
    public class AssignmentService
    {
        private readonly AssignmentRepository _repository;

        public AssignmentService(AssignmentRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Assignment>> GetAssignmentsAsync()
        {
            return _repository.GetAssignmentsAsync();
        }

        public Task<List<Assignment>> GetAssignmentsByCourseAsync(int courseId)
        {
            return _repository.GetAssignmentsByCourseAsync(courseId);
        }

        public Task<Assignment?> GetAssignmentByIdAsync(int id)
        {
            return _repository.GetAssignmentByIdAsync(id);
        }

        public Task AddAssignmentAsync(Assignment assignment)
        {
            return _repository.AddAssignmentAsync(assignment);
        }

        public Task UpdateAssignmentAsync(Assignment assignment)
        {
            return _repository.UpdateAssignmentAsync(assignment);
        }

        public Task RemoveAssignmentAsync(int id)
        {
            return _repository.RemoveAssignmentAsync(id);
        }
    }
}
