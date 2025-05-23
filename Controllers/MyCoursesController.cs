using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyTracker.Services;
using StudyTracker.ViewModels;

[Authorize(Roles = "Student")]
public class MyCoursesController : Controller
{
    private readonly StudentCourseAssignmentService _assignmentService;
    private readonly CourseService _courseService;
    private readonly UserManager<IdentityUser> _userManager;

    public MyCoursesController(
        StudentCourseAssignmentService assignmentService,
        CourseService courseService,
        UserManager<IdentityUser> userManager)
    {
        _assignmentService = assignmentService;
        _courseService = courseService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        var assignedCourseIds = (await _assignmentService.GetAssignmentsByStudentAsync(user.Id))
                                .Select(a => a.CourseId)
                                .ToList();

        var allCourses = await _courseService.GetCoursesAsync();
        var courses = allCourses.Where(c => assignedCourseIds.Contains(c.Id)).ToList();

        var vm = new CoursesVm(courses);
        return View("../Course/StudentIndex", vm);
    }
}
