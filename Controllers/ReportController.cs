using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyTracker.Services;
using System.Threading.Tasks;

[Authorize]
public class ReportController : Controller
{
    private readonly IReportService _reportService;
    private readonly UserManager<IdentityUser> _userManager;

    public ReportController(IReportService reportService, UserManager<IdentityUser> userManager)
    {
        _reportService = reportService;
        _userManager = userManager;
    }

    // Студент скачивает список своих заданий по курсу в docx
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> DownloadAssignmentsReport(int courseId)
    {
        var userId = _userManager.GetUserId(User);
        var reportBytes = await _reportService.GenerateAssignmentsDocxAsync(userId, courseId);
        return File(reportBytes,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "AssignmentsReport.docx");
    }

    // Админ скачивает Excel со списком просроченных студентов
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DownloadOverdueReport()
    {
        var reportBytes = await _reportService.GenerateOverdueStudentsXlsxAsync();
        return File(reportBytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "OverdueStudentsReport.xlsx");
    }
}
