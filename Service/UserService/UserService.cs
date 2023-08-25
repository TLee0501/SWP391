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

        public async Task<int> BanUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return 1;
            if (user.IsBan == true) return 2;
            try
            {
                user.IsBan = true;
                await _context.SaveChangesAsync();
                return 3;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> CreateAccount(CreateAccountRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(a => a.Email == request.Email);
            if (user != null) return -1;

            var roleName = await _context.Roles.FindAsync(request.RoleId);
            if (roleName.RoleName == "Student")
            {
                var mssvMax = await _context.Users.MaxAsync(a => a.Mssv);
                string digits = new string(mssvMax.Where(char.IsDigit).ToArray());
                string letters = new string(mssvMax.Where(char.IsLetter).ToArray());

                int number;
                if (!int.TryParse(digits, out number)) //int.Parse would do the job since only digits are selected
                {
                    Console.WriteLine("Something weired happened");
                }

                string newMSSV = letters + (++number).ToString("D4");

                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    FullName = request.FullName,
                    Email = request.Email,
                    Password = request.Password,
                    RoleId = request.RoleId,
                    Mssv = newMSSV,
                    IsBan = false
                };
                await _context.Users.AddAsync(newUser);
            }
            else
            {
                var newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    FullName = request.FullName,
                    Email = request.Email,
                    Password = request.Password,
                    RoleId = request.RoleId,
                    IsBan = false
                };
                await _context.Users.AddAsync(newUser);
            }

            await _context.SaveChangesAsync();
            return 0;
        }

        /*public async Task<int> CreateStudent(UserCreateRequest request)
        {
            var check = await _context.Users.SingleOrDefaultAsync(a => a.Email == request.Email);
            if (check != null) return 1;

            var role = await _context.Roles.SingleOrDefaultAsync(a => a.RoleName == "Student");

            
            await _context.SaveChangesAsync();
            return 2;
        }*/

        /*public async Task<int> CreateTeacher(UserCreateRequest request)
        {
            var check = await _context.Users.SingleOrDefaultAsync(a => a.Email == request.Email);
            if (check != null) return 1;
            var role = await _context.Roles.SingleOrDefaultAsync(a => a.RoleName == "Teacher");
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
        }*/

        public async Task<UserResponse> GetUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return null;
            var role = await _context.Roles.FindAsync(user.RoleId);
            UserResponse result = new UserResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = role.RoleName,
                Mssv = user.Mssv,
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
                tmp.Role = _context.Roles.FindAsync(user.RoleId).Result.RoleName;
                tmp.isBan = user.IsBan;
                result.Add(tmp);
            }
            if (result.Count == 0) return null;
            return result;
        }

        public async Task<UserResponse> Login(LoginRequest request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(a => a.Email == request.Email && a.Password == request.Password);

            if (user == null) return null;
            var role = await _context.Roles.FindAsync(user.RoleId);
            UserResponse result = new UserResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Mssv = user.Mssv,
                Role = role.RoleName,
                IsBan = user.IsBan
            };
            return result;
        }

        public async Task<List<UserListResponse>> SearchUser(string? search)
        {
            var searchValue = search?.ToLower();
            var query = _context.Users.Include(_ => _.Role).AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(_ => _.FullName.Contains(searchValue) || _.Email.Contains(searchValue));
            }
            var result = await query.ToListAsync();
            var users = result.Select(item => new UserListResponse
            {
                UserId = item.UserId,
                FullName = item.FullName,
                Email = item.Email,
                Role = item.Role.RoleName,
                RoleId = item.Role.RoleId,
                Mssv = item.Mssv,
                isBan = item.IsBan
            }).ToList();

            return users;
        }

        public async Task<int> UnbanUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return 1;
            if (user.IsBan == false) return 2;
            try
            {
                user.IsBan = false;
                await _context.SaveChangesAsync();
                return 3;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> UpdateUserRole(Guid userId, Guid roleId)
        {
            try
            {
                var user = await _context.Users.SingleAsync(_ => _.UserId == userId);
                if (user == null) return false;

                user.RoleId = roleId;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
