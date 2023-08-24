using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.UserService
{
    public interface IUserService
    {
        Task<UserResponse> Login(LoginRequest request);
        Task<List<UserListResponse>> ListTeacher();
        //Task<int> CreateStudent(UserCreateRequest request);
        Task<UserResponse> GetUser(Guid userId);
        Task<List<UserListResponse>> SearchUser(string? txtSearch);
        Task<int> BanUser(Guid userId);
        Task<int> UnbanUser(Guid userId);
        //Task<int> CreateTeacher(UserCreateRequest request);
        Task<bool> UpdateUserRole(Guid userId, Guid roleId);
        Task<int> CreateAccount(CreateAccountRequest request);
    }
}
