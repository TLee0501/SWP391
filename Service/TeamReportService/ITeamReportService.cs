using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.TeamReportService
{
    public interface ITeamReportService
    {
        public Task<bool> CreateTeamReport(Guid reporterId, CreateTeamReportRequest request);
        public Task<List<TeamReportDetailResponse>> GetTeamReports(Guid teamId);
        public Task<TeamReportDetailResponse?> GetTeamReport(Guid reportId);
    }
}

