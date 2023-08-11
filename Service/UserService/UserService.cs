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

        public async Task<List<UserListResponse>> ListTeacher()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(a => a.RoleName == "Teacher");
            var users = await _context.Users.Where(a => a.RoleId == role.RoleId).ToListAsync();
            List<UserListResponse> result = new List<UserListResponse>();
            foreach (var user in users)
            {
                var tmp = new UserListResponse();
                tmp.UserId = user.UserId;
                tmp.FullName = user.FullName;
                tmp.Email = user.Email;
                result.Add(tmp);
            }
            if (result.Count == 0) return null;
            return result;
        }

        public async Task<UserResponse> Login(LoginRequest request)
        {
            var user = _context.Users.SingleOrDefaultAsync(a => a.Email == request.Mail && a.Password == request.Password);

            if (user == null) return null;
            var role = _context.Roles.FindAsync(user.Result.RoleId);
            UserResponse result = new UserResponse
            {
                UserId = user.Result.UserId,
                FullName = user.Result.FullName,
                Email = user.Result.Email,
                Role = role.Result.RoleName,
                IsBan = user.Result.IsBan
            };

            return result;
        }
    }
}
