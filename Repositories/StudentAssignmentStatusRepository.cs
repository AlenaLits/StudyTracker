using Microsoft.EntityFrameworkCore;
using StudyTracker.Data;
using StudyTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyTracker.Repositories
{
    public class StudentAssignmentStatusRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentAssignmentStatusRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentAssignmentStatus>> GetStatusesByStudentAsync(string studentId)
        {
            return await _context.StudentAssignmentStatuses.Where(s => s.StudentId == studentId).ToListAsync();
        }

        public async Task<List<StudentAssignmentStatus>> GetStatusesAsync()
        {
            return await _context.StudentAssignmentStatuses.ToListAsync();
        }

        public async Task<StudentAssignmentStatus?> GetStatusAsync(string studentId, int assignmentId)
        {
            return await _context.StudentAssignmentStatuses.FirstOrDefaultAsync(s => s.StudentId == studentId && s.AssignmentId == assignmentId);
        }

        public async Task AddOrUpdateStatusAsync(StudentAssignmentStatus status)
        {
            var existing = await GetStatusAsync(status.StudentId, status.AssignmentId);
            if (existing == null)
            {
                _context.StudentAssignmentStatuses.Add(status);
            }
            else
            {
                existing.Status = status.Status;
                _context.StudentAssignmentStatuses.Update(existing);
            }
            await _context.SaveChangesAsync();
        }
    }
}
