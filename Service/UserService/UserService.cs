using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace Service.UserService
{
    public class UserService : IUserService
    {
        private readonly Swp391onGoingReportContext _context;

        public UserService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<UserResponse> Login(LoginRequest request)
        {
            var user = _context.Users.SingleOrDefaultAsync(a => a.Email == request.Mail && a.Password == request.Password);

            if (user == null) return null;

            UserResponse result = new UserResponse
            {
                UserId = user.Result.UserId,
                FullName = user.Result.FullName,
                Email = user.Result.Email,
                Role = _context.Roles.FindAsync(user.Result.RoleId).Result.RoleName,
                IsBan = user.Result.IsBan
            };

            return result;
        }
    }
}
