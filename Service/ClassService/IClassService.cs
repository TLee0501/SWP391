using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.ClassService
{
    public interface IClassService
    {
        Task<int> CreateClass(CreateClassRequest request);
        Task<ClassDetailResponse?> GetClassByID(Guid classId, Guid? userId);
        Task<int> UpdateClass(UpdateClassRequest request);
        Task<int> DeleteClass(Guid classId);
        Task<int> AssignClass(AssignClassRequest request);
        Task<int> UnassignClass(AssignClassRequest request);
        Task<List<ClassListResponse>> GetClasses(Guid userId, Guid? teacherId, Guid? semesterId, Guid? courseId, string? searchText);
        Task<bool> EnrollClass(Guid userId, Guid classId, string enrollCode);
        Task<List<UserListResponse>> GetUsersInClass(Guid classId);
        Task<List<ClassListResponse>> GetTeacherClassList(Guid teacherId);
        Task<List<UserListResponse>> GetStudentsNotInProjectInClass(Guid classId);
        Task<bool> UpdateTeamRegisterDeadline(UpdateClassDeadlineRequest request);
        Task<bool> UpdateReportDeadline(UpdateClassDeadlineRequest request);
        Task<bool> UpdateEnrollCode(UpdateEnrollCodeRequest request);
    }
}
