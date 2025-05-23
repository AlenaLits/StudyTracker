using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyTracker.Models;
using StudyTracker.Services;
using StudyTracker.ViewModelBuilders;

[Authorize(Roles = "Administrator")]
public class CourseController : Controller
{
    private readonly CourseService _courseService;
    private readonly CoursesVmBuilder _coursesVmBuilder;
    private readonly StudentCourseAssignmentService _assignmentService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly CoursesWithStudentsVmBuilder _coursesWithStudentsVmBuilder;

    public CourseController(
        CourseService courseService,
        CoursesVmBuilder coursesVmBuilder,
        StudentCourseAssignmentService assignmentService,
        UserManager<IdentityUser> userManager,
        CoursesWithStudentsVmBuilder coursesWithStudentsVmBuilder)
    {
        _courseService = courseService;
        _coursesVmBuilder = coursesVmBuilder;
        _assignmentService = assignmentService;
        _userManager = userManager;
        _coursesWithStudentsVmBuilder = coursesWithStudentsVmBuilder;
    }

    public async Task<IActionResult> Index()
    {
        var vm = await _coursesWithStudentsVmBuilder.GetCoursesWithStudentsVmAsync();
        return View(vm);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Course course)
    {
        if (ModelState.IsValid)
        {
            await _courseService.AddCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }
        return View(course);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
            return NotFound();
        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Course course)
    {
        if (id != course.Id)
            return BadRequest();

        if (ModelState.IsValid)
        {
            await _courseService.UpdateCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }
        return View(course);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var course = await _courseService.GetCourseByIdAsync(id);
        if (course == null)
            return NotFound();
        return View(course);
    }

    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _courseService.RemoveCourseAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Assign(int courseId)
    {
        var course = await _courseService.GetCourseByIdAsync(courseId);
        if (course == null) return NotFound();

        var users = _userManager.Users.ToList();
        var students = new List<IdentityUser>();

        foreach (var user in users)
        {
            if (await _userManager.IsInRoleAsync(user, "Student"))
                students.Add(user);
        }

        ViewBag.Course = course;
        ViewBag.Students = students;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(int courseId, string studentId)
    {
        if (string.IsNullOrEmpty(studentId))
        {
            ModelState.AddModelError("", "Выберите студента");
            return RedirectToAction(nameof(Assign), new { courseId });
        }

        await _assignmentService.AssignCourseToStudentAsync(studentId, courseId);
        return RedirectToAction(nameof(Index));
    }
}
