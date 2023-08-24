using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using Service.UserService;
using BusinessObjects.ResponseModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Swp391onGoingReportContext _context;
        private readonly IUserService _userService;
        private IConfiguration _configuration;

        public UsersController(Swp391onGoingReportContext context, IUserService userService, IConfiguration configuration)
        {
            _context = context;
            _userService = userService;
            _configuration = configuration;
        }


        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersForTest()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (request.Email == "" || request.Email == null)
                return BadRequest("Vui lòng kiểm tra lại Mail!");
            if (request.Password == "" || request.Password == null)
                return BadRequest("Vui lòng kiểm tra lại mật khẩu!");

            var result = await _userService.Login(request);

            if (result == null)
                return NotFound("Vui lòng kiểm tra lại số điện thoại/mật khẩu!");
            else if (result.IsBan == true)
                return Unauthorized("Tài khoản đã bị khóa!");

            var token = CreateToken(result);
            return Ok(new LoginResponse<UserResponse>(200, "Đăng nhập thành công.", result, token));
        }

        // GET: api/Users/5
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<UserListResponse>>> GetListTeacher()
        {
            try
            {
                var result = await _userService.ListTeacher();
                if (result == null) return BadRequest("Không có giảng viên!");
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest("Thất bại!"); }
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<UserResponse>> GetUser()
        {
            try
            {
                var roleClaim = User?.FindAll(ClaimTypes.Name);
                var userID = new Guid(roleClaim?.Select(c => c.Value).SingleOrDefault().ToString());

                var result = await _userService.GetUser(userID);
                if (result == null) return BadRequest("Tài khoản không tồn tại!");
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest("Thất bại!"); }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{userId}")]
        public async Task<IActionResult> UnbanUser(Guid userId)
        {
            var result = await _userService.UnbanUser(userId);
            if (result == 1) return BadRequest("Tài khoản không tồn tại!");
            else if (result == 2) return BadRequest("Tài khoản đã ở trạng thái hoạt động!");
            else if (result == 3) return Ok("Thành công!");
            else return BadRequest("Thất bại!");
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> BanUser(Guid userId)
        {
            var result = await _userService.BanUser(userId);
            if (result == 1) return BadRequest("Tài khoản không tồn tại!");
            else if (result == 2) return BadRequest("Tài khoản đã ở trạng thái bị khóa!");
            else if (result == 3) return Ok("Thành công!");
            else return BadRequest("Thất bại!");
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost]
        public async Task<ActionResult> CreateStudent(UserCreateRequest request)
        {
            try
            {
                var result = await _userService.CreateStudent(request);
                if (result == 0) return BadRequest("Thất bại!");
                else if (result == 1) return BadRequest("Email đã được sử dụng!");
                return Ok("Thành công!");
            }
            catch (Exception ex) { return BadRequest("Thất bại!"); }
        }*/

        /*[HttpPost]
        public async Task<ActionResult> CreateTeacher(UserCreateRequest request)
        {
            try
            {
                var result = await _userService.CreateTeacher(request);
                if (result == 0) return BadRequest("Thất bại!");
                else if (result == 1) return BadRequest("Email đã được sử dụng!");
                return Ok("Thành công!");
            }
            catch (Exception ex) { return BadRequest("Thất bại!"); }
        }*/

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserListResponse>>> SearchUser(string? search)
        {
            var result = await _userService.SearchUser(search);
            return Ok(result);
        }

        // DELETE: api/Users/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private string CreateToken(UserResponse request)
        {
            List<Claim> claims = new List<Claim>
            {
                //new Claim(ClaimTypes.Name, request.Email),
                new Claim(ClaimTypes.Name, request.UserId.ToString()),
                new Claim(ClaimTypes.Role, request.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Appsettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateUserRole(UpdateRoleRequest request)
        {
            var success = await _userService.UpdateUserRole(request.UserId, request.RoleId);
            if (success)
            {
                return Ok("Cập nhật role thành công.");
            }
            return BadRequest("Cập nhật role thất bại.");
        }

        [HttpPost]
        public async Task<ActionResult> Register(CreateAccountRequest request)
        {
            try
            {
                var result = await _userService.CreateAccount(request);

                if (result == -1) return BadRequest("Email đã được sử dụng!");

                return Ok("Thành công!");
            }
            catch (Exception e)
            {
                return BadRequest("Thất bại!");
            }
        }
    }
}