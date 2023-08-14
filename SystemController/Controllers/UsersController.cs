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
                return BadRequest("Vui lòng kiểm tra lại Email!");
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

        [HttpGet]
        public async Task<ActionResult<UserResponse>> GetUser(Guid userId)
        {
            try
            {
                var result = await _userService.GetUser(userId);
                if (result == null) return BadRequest("Tài khoản không tồn tại!");
                return Ok(result);
            }
            catch (Exception ex) { return BadRequest("Thất bại!"); }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
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
                new Claim(ClaimTypes.Name, request.Email),
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
    }
}
