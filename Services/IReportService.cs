namespace StudyTracker.Services
{
    public interface IReportService
    {
        Task<byte[]> GenerateAssignmentsDocxAsync(string studentId, int courseId);
        Task<byte[]> GenerateOverdueStudentsXlsxAsync();
    }
}
