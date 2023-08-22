using BusinessObjects.ResponseModel;

namespace Service.OnGoingReportService
{
    public interface IOnGoingReportService
    {
        public Task<List<ReportTaskResponse>> GetOnGoingReportInClass(Guid classId);
    }
}
