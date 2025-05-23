using Microsoft.EntityFrameworkCore;
using StudyTracker.Data;
using StudyTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyTracker.Repositories
{
    public class StudentCourseAssignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentCourseAssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentCourseAssignment>> GetAssignmentsAsync()
        {
            return await _context.StudentCourseAssignments.ToListAsync();
        }

        public async Task<List<StudentCourseAssignment>> GetAssignmentsByStudentAsync(string studentId)
        {
            return await _context.StudentCourseAssignments.Where(a => a.StudentId == studentId).ToListAsync();
        }

        public async Task AddAssignmentAsync(StudentCourseAssignment assignment)
        {
            _context.StudentCourseAssignments.Add(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAssignmentAsync(StudentCourseAssignment assignment)
        {
            _context.StudentCourseAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string studentId, int courseId)
        {
            return await _context.StudentCourseAssignments.AnyAsync(a => a.StudentId == studentId && a.CourseId == courseId);
        }
    }
}
