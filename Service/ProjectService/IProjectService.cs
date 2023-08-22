using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.ProjectService
{
    public interface IProjectService
    {
        Task<ProjectResponse> GetProjectByID(Guid projectId);
        Task<int> CreateProject(ProjectCreateRequest request);
        Task<int> UpdateProject(ProjectUpdateRequest request);
        Task<List<ProjectResponse>> GetProjectsByClassID(Guid classId);
        //Task<List<ProjectResponse>> SearchProjectInClass(Guid classId, string? searchName);
        //Task<List<ProjectResponse>> GetProjectsByClassIDandUserID(Guid classId, Guid userId);
        Task<List<ProjectResponse>> GetProjectsByFilter(Guid classId, Guid userId, string? searchName, bool hasUserId);
        Task<int> DeleteProject(Guid projectId);
        Task<List<ProjectResponse>> GetAllProjectsInClass(Guid classId, string? searchName);
        Task<List<ProjectResponse>> GetWorkingProjectsInClass(Guid userId, Guid classId);
    }
}
