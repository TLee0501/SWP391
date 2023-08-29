using System.Threading.Tasks;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.ProjectTeamService
{
    public interface IProjectTeamServise
    {
        Task<ProjectTeamResponse> getProjectTeamById(Guid projectTeamId);
        //Task<int> StudentCreateTeamRequest(StudentCreateTeamRequest request);
        //Task<List<TeamRequestResponse>> GetTeamProjectRequests(Guid? userId, Guid classId);
        //Task<int> AcceptTeamProjectrequest(Guid teamId);
        //Task<int> DenyTeamProjectrequest(Guid teamId);
        //Task<int> CancelProjectrequest(Guid teamId);
        Task<List<ProjectTeamResponse>> getProjectTeamInClass(Guid classId);
        //Task<int> DeleteProjectTeam(Guid projectTeamId);
        Task<int> CreateTeam(Guid leaderId, ProjectTeamCreateRequest request);
        Task<List<ProjectTeamListResponse>> GetJoinedProjectTeams(Guid userId, Guid classId);
        Task<ProjectTeamDetailResponse?> GetJoinedProjectTeamById(Guid userId, Guid teamId);
        Task<int> RemoveMember(Guid projectTeamId, Guid userId);
        Task<int> AddMember(Guid projectTeamId, Guid userId);
        Task<List<ProjectTeamListResponse>> GetProjectTeamsByTeacher(Guid teacherId);
        Task<ProjectTeamDetailResponse> GetProjectTeamDetailByTeacher(Guid teamId);
    }
}
