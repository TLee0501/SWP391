using BusinessObjects.Models;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace Service.OnGoingReportService
{
    public class OnGoingReportService : IOnGoingReportService
    {
        private readonly Swp391onGoingReportContext _context;

        public OnGoingReportService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<List<ReportTaskResponse>> GetOnGoingReportInClass(Guid classId)
        {
            var result = new List<ReportTaskResponse>();
            var projects = await _context.Projects.Where(a => a.ClassId == classId && a.IsDeleted == false).ToListAsync();
            foreach (var item in projects)
            {
                var report = new ReportTaskResponse();
                var total = await _context.Tasks.Where(a => a.ProjectId == item.ProjectId && a.IsDeleted == false).ToListAsync();
                report.NumberOfTask = total.Count();

                var done = await _context.Tasks.Where(a => a.ProjectId == item.ProjectId && a.IsDeleted == false && a.Status.Equals(1)).ToListAsync();
                report.NumberOfFinishTask = done.Count();

                var doing = await _context.Tasks.Where(a => a.ProjectId == item.ProjectId && a.IsDeleted == false && a.Status.Equals(0)).ToListAsync();
                report.NumberOfDoingTask = doing.Count();

                var undone = await _context.Tasks.Where(a => a.ProjectId == item.ProjectId && a.IsDeleted == false && a.Status.Equals(2)).ToListAsync();
                report.NumberOfUnFinishTask = undone.Count();

                report.ProjectId = item.ProjectId;
                report.ProjectName = item.ProjectName;
                result.Add(report);
            }
            return result;
        }
    }
}
