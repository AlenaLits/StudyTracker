using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyTracker.Models;
using StudyTracker.Services;
using StudyTracker.ViewModels;

[Authorize]
public class AssignmentController : Controller
{
    private readonly StudentCourseAssignmentService _studentCourseAssignmentService;
    private readonly AssignmentService _assignmentService;
    private readonly CourseService _courseService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly StudentAssignmentStatusService _studentAssignmentStatusService;

    public AssignmentController(
        AssignmentService assignmentService,
        CourseService courseService,
        StudentCourseAssignmentService studentCourseAssignmentService,
        StudentAssignmentStatusService studentAssignmentStatusService,
        UserManager<IdentityUser> userManager)
    {
        _assignmentService = assignmentService;
        _courseService = courseService;
        _studentCourseAssignmentService = studentCourseAssignmentService;
        _studentAssignmentStatusService = studentAssignmentStatusService;
        _userManager = userManager;
    }

    [Authorize(Roles = "Administrator,Student")]
    public async Task<IActionResult> Index(int courseId, string? statusFilter, string? sortOrder)
    {
        var user = await _userManager.GetUserAsync(User);
        var isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");

        var assignments = await _assignmentService.GetAssignmentsByCourseAsync(courseId);
        // Фильтрация по статусу (если выбрана)
        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "Все")
        {
            assignments = assignments.Where(a => a.Status == statusFilter).ToList();
        }

        // Сортировка по дедлайну
        assignments = sortOrder switch
        {
            "deadline_asc" => assignments.OrderBy(a => a.Deadline).ToList(),
            "deadline_desc" => assignments.OrderByDescending(a => a.Deadline).ToList(),
            _ => assignments
        };
        ViewBag.StatusFilter = statusFilter;
        ViewBag.SortOrder = sortOrder;
        if (isAdmin)
        {
            var course = await _courseService.GetCourseByIdAsync(courseId);
            var vm = new AssignmentsVm(courseId, course?.Name ?? "Course", assignments);
            return View(vm);
        }
        else
        {
            var assignedCourses = (await _studentCourseAssignmentService.GetAssignmentsByStudentAsync(user.Id))
                                  .Select(a => a.CourseId);
            if (!assignedCourses.Contains(courseId))
                return Forbid();

            var studentStatuses = await _studentAssignmentStatusService.GetStatusesByStudentAsync(user.Id);

            var personalizedAssignments = assignments.Select(a =>
            {
                var status = studentStatuses.FirstOrDefault(s => s.AssignmentId == a.Id)?.Status ?? "Не начато";
                return new Assignment
                {
                    Id = a.Id,
                    CourseId = a.CourseId,
                    Name = a.Name,
                    Description = a.Description,
                    Deadline = a.Deadline,
                    Status = status
                };
            }).ToList();

            var course = await _courseService.GetCourseByIdAsync(courseId);
            var vm = new AssignmentsVm(courseId, course?.Name ?? "Course", personalizedAssignments);
            return View(vm);
        }
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(int courseId)
    {
        var course = await _courseService.GetCourseByIdAsync(courseId);
        if (course == null)
            return NotFound();

        var assignment = new Assignment { CourseId = courseId };
        ViewBag.CourseName = course.Name;
        return View(assignment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(Assignment assignment)
    {
        if (assignment.CourseId == 0)
        {
            ModelState.AddModelError("CourseId", "CourseId не передан");
        }
        try
        {
            await _assignmentService.AddAssignmentAsync(assignment);
            return RedirectToAction(nameof(Index), new { courseId = assignment.CourseId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
            var course = await _courseService.GetCourseByIdAsync(assignment.CourseId);
            ViewBag.CourseName = course?.Name;
            return View(assignment);
        }
        //if (ModelState.IsValid)
        //{
        //    await _assignmentService.AddAssignmentAsync(assignment);
        //    return RedirectToAction(nameof(Index), new { courseId = assignment.CourseId });
        //}
        //var course = await _courseService.GetCourseByIdAsync(assignment.CourseId);
        //ViewBag.CourseName = course?.Name;
        //return View(assignment);
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(int id)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
        if (assignment == null)
            return NotFound();

        var course = await _courseService.GetCourseByIdAsync(assignment.CourseId);
        ViewBag.CourseName = course?.Name;
        return View(assignment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(int id, Assignment assignment)
    {
        if (assignment.CourseId == 0)
        {
            ModelState.AddModelError("CourseId", "CourseId не передан");
        }
        if (id != assignment.Id)
            return BadRequest();

        try
        {
            await _assignmentService.UpdateAssignmentAsync(assignment);
            return RedirectToAction(nameof(Index), new { courseId = assignment.CourseId });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
            var course = await _courseService.GetCourseByIdAsync(assignment.CourseId);
            ViewBag.CourseName = course?.Name;
            return View(assignment);
        }
        
    }

    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Delete(int id)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
        if (assignment == null)
            return NotFound();

        var course = await _courseService.GetCourseByIdAsync(assignment.CourseId);
        ViewBag.CourseName = course?.Name;
        return View(assignment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
        if (assignment != null)
        {
            await _assignmentService.RemoveAssignmentAsync(id);
            return RedirectToAction(nameof(Index), new { courseId = assignment.CourseId });
        }
        return NotFound();
    }

    [HttpPost]
    [Authorize(Roles = "Administrator,Student")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangeStatus(int id, string status)
    {
        var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
        if (assignment == null)
            return NotFound();

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");

        if (isAdmin)
        {
            assignment.Status = status;
            await _assignmentService.UpdateAssignmentAsync(assignment);
        }
        else
        {
            var studentStatus = await _studentAssignmentStatusService.GetStatusAsync(user.Id, id)
                ?? new StudentAssignmentStatus { StudentId = user.Id, AssignmentId = id };

            studentStatus.Status = status;
            await _studentAssignmentStatusService.AddOrUpdateStatusAsync(studentStatus);
        }

        return RedirectToAction(nameof(Index), new { courseId = assignment.CourseId });
    }
}
