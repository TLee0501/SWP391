using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.UserService
{
    public interface IUserService
    {
        Task<UserResponse> Login(LoginRequest request);
        Task<List<UserListResponse>> ListTeacher();
        Task<int> CreateStudent(UserCreateRequest request);
        Task<UserResponse> GetUser(Guid userId);
    }
}
