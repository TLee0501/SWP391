using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.ProjectTeamService
{
    public interface IProjectTeamServise
    {
        Task<ProjectTeamResponse> getProjectTeamById(Guid projectTeamId);
        Task<int> StudentCreateTeamRequest(StudentCreateTeamRequest request);
        Task<List<TeamRequestResponse>> GetTeamProjectRequests(Guid classId);
        Task<int> AcceptTeamProjectrequest(Guid teamId);
        Task<int> DenyTeamProjectrequest(Guid teamId);
        Task<List<ProjectTeamResponse>> getProjectTeamInClass(Guid classId);
        Task<int> DeleteProjectTeam(Guid projectTeamId);
    }
}
