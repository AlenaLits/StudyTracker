using Microsoft.AspNetCore.Identity;
using StudyTracker.Models;
using System.Collections.Generic;

namespace StudyTracker.ViewModels
{
    public class CourseWithStudentsVm
    {
        public Course Course { get; set; }
        public List<IdentityUser> AssignedStudents { get; set; }

        public CourseWithStudentsVm()
        {
            AssignedStudents = new List<IdentityUser>();
        }
    }
}
