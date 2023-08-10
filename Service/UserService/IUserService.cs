using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.UserService
{
    public interface IUserService
    {
        Task<UserResponse> Login(LoginRequest request);
    }
}
