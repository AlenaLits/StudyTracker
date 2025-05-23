using StudyTracker.Models;
using StudyTracker.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;  // для ToListAsync()
using StudyTracker.ViewModels;

namespace StudyTracker.ViewModelBuilders
{
    public class CoursesWithStudentsVmBuilder
    {
        private readonly CourseService _courseService;
        private readonly StudentCourseAssignmentService _assignmentService;
        private readonly UserManager<IdentityUser> _userManager;

        public CoursesWithStudentsVmBuilder(
            CourseService courseService,
            StudentCourseAssignmentService assignmentService,
            UserManager<IdentityUser> userManager)
        {
            _courseService = courseService;
            _assignmentService = assignmentService;
            _userManager = userManager;
        }

        public async Task<CoursesWithStudentsVm> GetCoursesWithStudentsVmAsync()
        {
            var courses = await _courseService.GetCoursesAsync();
            var allStudents = await _userManager.Users.ToListAsync();
            var vm = new CoursesWithStudentsVm();

            foreach (var course in courses)
            {
                var assignments = (await _assignmentService.GetAssignmentsAsync())
                    .Where(a => a.CourseId == course.Id)
                    .ToList();

                var assignedStudentIds = assignments
                    .Select(a => a.StudentId)
                    .Distinct()
                    .ToList();

                var studentsForCourse = allStudents
                    .Where(s => assignedStudentIds.Contains(s.Id))
                    .ToList();

                vm.Courses.Add(new CourseWithStudentsVm
                {
                    Course = course,
                    AssignedStudents = studentsForCourse
                });
            }

            return vm;
        }
    }
}
