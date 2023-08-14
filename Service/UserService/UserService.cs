using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Service.UserService
{
    public class UserService : IUserService
    {
        private readonly Swp391onGoingReportContext _context;

        public UserService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<int> CreateStudent(UserCreateRequest request)
        {
            var check = await _context.Users.SingleOrDefaultAsync(a => a.Email == request.Email);
            if (check != null) return 1;
            var role = await _context.Roles.SingleOrDefaultAsync(a => a.RoleName == "Student");
            var user = new User
            {
                UserId = Guid.NewGuid(),
                FullName = request.FullName,
                Email = request.Email,
                Password = request.Password,
                RoleId = role.RoleId,
                IsBan = false
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return 2;
        }

        public async Task<UserResponse> GetUser(Guid userID)
        {
            var user = await _context.Users.FindAsync(userID);
            if (user == null) return null;
            var role = await _context.Roles.FindAsync(user.RoleId);
            UserResponse result = new UserResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = role.RoleName,
                IsBan = user.IsBan
            };
            return result;
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
            var user = await _context.Users.SingleOrDefaultAsync(a => a.Email == request.Mail && a.Password == request.Password);

            if (user == null) return null;
            var role = await _context.Roles.FindAsync(user.RoleId);
            UserResponse result = new UserResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = role.RoleName,
                IsBan = user.IsBan
            };
            return result;
        }

        public async Task<List<UserListResponse>> SearchUser(string txtSearch)
        {
            var result = new List<UserListResponse>();
            var users = await _context.Users.ToListAsync();
            foreach (var item in users)
            {
                if (item.FullName.ToLower().Contains(txtSearch.ToLower()))
                {
                    var tmp = new UserListResponse
                    {
                        UserId = item.UserId,
                        FullName = item.FullName,
                        Email = item.Email
                    };
                    result.Add(tmp);
                }
            }
            return result;
        }
    }
}
