using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.ProjectService
{
    public interface IProjectService
    {
        Task<ProjectResponse> GetProjectByID(Guid projectID);
        Task<int> CreateProject(ProjectCreateRequest request);
        Task<int> UpdateProject(ProjectUpdateRequest request);
        Task<List<ProjectResponse>> GetProjectsByClassID(Guid classID);
        Task<List<ProjectResponse>> SearchProjectInClass(Guid classID, string searchName);
    }
}
