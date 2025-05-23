using StudyTracker.Models;
using System.Collections.Generic;

namespace StudyTracker.ViewModels
{
    public class AssignmentsVm
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<Assignment> Assignments { get; set; }

        public AssignmentsVm(int courseId, string courseName, List<Assignment> assignments)
        {
            CourseId = courseId;
            CourseName = courseName;
            Assignments = assignments;
        }
    }
}
