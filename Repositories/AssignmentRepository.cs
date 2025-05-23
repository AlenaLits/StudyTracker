using Microsoft.EntityFrameworkCore;
using StudyTracker.Data;
using StudyTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudyTracker.Repositories
{
    public class AssignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Assignment>> GetAssignmentsAsync()
        {
            return await _context.Assignments.ToListAsync();
        }

        public async Task<List<Assignment>> GetAssignmentsByCourseAsync(int courseId)
        {
            return await _context.Assignments.Where(a => a.CourseId == courseId).ToListAsync();
        }

        public async Task<Assignment?> GetAssignmentByIdAsync(int id)
        {
            return await _context.Assignments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task AddAssignmentAsync(Assignment assignment)
        {
            if (assignment.Deadline.HasValue)
            {
                assignment.Deadline = DateTime.SpecifyKind(assignment.Deadline.Value, DateTimeKind.Utc);
            }
            _context.Assignments.Add(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAssignmentAsync(Assignment updatedAssignment)
        {
            if (updatedAssignment.Deadline.HasValue)
            {
                updatedAssignment.Deadline = DateTime.SpecifyKind(updatedAssignment.Deadline.Value, DateTimeKind.Utc);
            }

            _context.Assignments.Update(updatedAssignment);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAssignmentAsync(int id)
        {
            var assignment = await GetAssignmentByIdAsync(id);
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                await _context.SaveChangesAsync();
            }
        }
    }
}
