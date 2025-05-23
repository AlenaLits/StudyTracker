using StudyTracker.Models;
using StudyTracker.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTracker.Services
{
    public class StudentAssignmentStatusService
    {
        private readonly StudentAssignmentStatusRepository _repository;

        public StudentAssignmentStatusService(StudentAssignmentStatusRepository repository)
        {
            _repository = repository;
        }

        public Task<StudentAssignmentStatus?> GetStatusAsync(string studentId, int assignmentId)
        {
            return _repository.GetStatusAsync(studentId, assignmentId);
        }

        public Task<List<StudentAssignmentStatus>> GetStatusesAsync()
        {
            return _repository.GetStatusesAsync();
        }

        public Task AddOrUpdateStatusAsync(StudentAssignmentStatus status)
        {
            return _repository.AddOrUpdateStatusAsync(status);
        }

        public Task<List<StudentAssignmentStatus>> GetStatusesByStudentAsync(string studentId)
        {
            return _repository.GetStatusesByStudentAsync(studentId);
        }
    }
}
