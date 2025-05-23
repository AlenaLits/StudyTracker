//using Xceed.Words.NET;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using StudyTracker.Models;
using StudyTracker.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

public class ReportService : IReportService
{
    private readonly AssignmentService _assignmentService;
    private readonly StudentAssignmentStatusService _studentAssignmentStatusService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly CourseService _courseService;

    public ReportService(
        AssignmentService assignmentService,
        StudentAssignmentStatusService studentAssignmentStatusService,
        UserManager<IdentityUser> userManager,
        CourseService courseService)
    {
        _assignmentService = assignmentService;
        _studentAssignmentStatusService = studentAssignmentStatusService;
        _userManager = userManager;
        _courseService = courseService;
    }

    // Генерация Word отчёта со списком заданий студента по курсу с его индивидуальным статусом
    public async Task<byte[]> GenerateAssignmentsDocxAsync(string studentId, int courseId)
    {
        var student = await _userManager.FindByIdAsync(studentId);
        var assignments = await _assignmentService.GetAssignmentsByCourseAsync(courseId);
        var studentStatuses = await _studentAssignmentStatusService.GetStatusesByStudentAsync(studentId);

        using var ms = new MemoryStream();

        using (var wordDocument = WordprocessingDocument.Create(ms, DocumentFormat.OpenXml.WordprocessingDocumentType.Document, true))
        {
            var mainPart = wordDocument.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());

            // Заголовок
            var paraTitle = new Paragraph(new Run(new Text($"Отчёт по заданиям студента: {student?.UserName ?? "Неизвестно"}")));
            body.AppendChild(paraTitle);

            // Пустая строка
            body.AppendChild(new Paragraph(new Run(new Text(""))));

            foreach (var assignment in assignments)
            {
                var status = studentStatuses.FirstOrDefault(s => s.AssignmentId == assignment.Id)?.Status ?? "Не начато";

                body.AppendChild(new Paragraph(new Run(new Text($"Название: {assignment.Name}"))) { ParagraphProperties = new ParagraphProperties(new Bold()) });
                body.AppendChild(new Paragraph(new Run(new Text($"Описание: {assignment.Description ?? "-"}"))));
                body.AppendChild(new Paragraph(new Run(new Text($"Статус: {status}"))));
                body.AppendChild(new Paragraph(new Run(new Text($"Дедлайн: {(assignment.Deadline?.ToString("dd.MM.yyyy") ?? "-")}"))));
                body.AppendChild(new Paragraph(new Run(new Text("")))); // пустая строка для разделения
            }

            mainPart.Document.Save();
        }

        return ms.ToArray();
    }

    // Генерация Excel отчёта с просроченными студентами (по индивидуальным статусам)
    public async Task<byte[]> GenerateOverdueStudentsXlsxAsync()
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Просроченные студенты");

        worksheet.Cell(1, 1).Value = "Курс";
        worksheet.Cell(1, 2).Value = "Студент";
        worksheet.Cell(1, 3).Value = "Задание";
        worksheet.Cell(1, 4).Value = "Дедлайн";

        int row = 2;

        var allAssignments = await _assignmentService.GetAssignmentsAsync();
        var allStatuses = await _studentAssignmentStatusService.GetStatusesAsync();

        foreach (var status in allStatuses)
        {
            if (status.Status != "Завершено")
            {
                var assignment = allAssignments.FirstOrDefault(a => a.Id == status.AssignmentId);
                if (assignment != null && assignment.Deadline.HasValue && assignment.Deadline.Value < DateTime.Now)
                {
                    var course = await _courseService.GetCourseByIdAsync(assignment.CourseId);
                    var student = await _userManager.FindByIdAsync(status.StudentId);

                    worksheet.Cell(row, 1).Value = course?.Name ?? "Неизвестно";
                    worksheet.Cell(row, 2).Value = student?.UserName ?? "Неизвестно";
                    worksheet.Cell(row, 3).Value = assignment.Name;
                    worksheet.Cell(row, 4).Value = assignment.Deadline?.ToString("dd.MM.yyyy") ?? "-";

                    row++;
                }
            }
        }

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        return ms.ToArray();
    }
}
