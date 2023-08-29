using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.TeamReportService
{
    public interface ITeamReportService
    {
        public Task<int> CreateTeamReport(Guid reporterId, CreateTeamReportRequest request);
        public Task<List<TeamReportDetailResponse>> GetTeamReports(Guid teamId);
        public Task<TeamReportDetailResponse?> GetTeamReport(Guid reportId);
        public Task<bool> CreateTeamReportFeedback(CreateTeamReportFeedback request);
    }
}

