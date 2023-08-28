using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.ProjectTeamService;
using System.Security.Claims;

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProjectTeamsController : ControllerBase
    {
        private readonly Swp391onGoingReportContext _context;
        private readonly IProjectTeamServise _projectTeamServise;

        public ProjectTeamsController(Swp391onGoingReportContext context, IProjectTeamServise projectTeamServise)
        {
            _context = context;
            _projectTeamServise = projectTeamServise;
        }

        // GET: api/ProjectTeams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTeam>>> GetProjectTeamsForTest()
        {
            if (_context.ProjectTeams == null)
            {
                return NotFound();
            }
            return await _context.ProjectTeams.ToListAsync();
        }

        // GET: api/ProjectTeams/5
        [HttpGet("{ProjectTeamId}")]
        public async Task<ActionResult<ProjectTeam>> GetProjectTeamById(Guid ProjectTeamId)
        {
            var result = await _projectTeamServise.getProjectTeamById(ProjectTeamId);
            if (result == null) return NotFound(new ResponseCodeAndMessageModel(10, "Không tìm thấy nhóm!"));
            return Ok(result);
        }

        [HttpGet("{classId}")]
        public async Task<ActionResult<ProjectTeam>> getProjectTeamInClass(Guid classId)
        {
            var result = await _projectTeamServise.getProjectTeamInClass(classId);
            if (result.IsNullOrEmpty()) return NotFound(new ResponseCodeAndMessageModel(10, "Không tìm thấy nhóm!"));
            return Ok(result);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<ProjectTeam>> RegisterTeam(ProjectTeamCreateRequest request)
        {
            var roleClaim = User?.FindAll(ClaimTypes.Name);
            var userID = new Guid(roleClaim?.Select(c => c.Value).SingleOrDefault().ToString());

            var result = await _projectTeamServise.CreateTeam(userID, request);
            if (result == 1) return NotFound(new ResponseCodeAndMessageModel(7, "Không tìm thấy dự án!"));
            else if (result == 2) return BadRequest(new ResponseCodeAndMessageModel(8, "Có thành viên bị lặp!"));
            else if (result == 3) return BadRequest(new ResponseCodeAndMessageModel(9, "Có thành viên đã tham gia nhóm khác!"));
            else if (result == 4) return Ok(new ResponseCodeAndMessageModel(100, "Thành công!"));
            else return BadRequest(new ResponseCodeAndMessageModel(99, "Thất bại!"));
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<ProjectTeam>> GetJoinedProjectTeams(Guid classId)
        {
            var userId = Utils.GetUserIdFromHttpContext(HttpContext);
            var teams = await _projectTeamServise.GetJoinedProjectTeams(new Guid(userId!), classId);
            return Ok(teams);
        }

        [HttpGet("{teamId}"), Authorize]
        public async Task<ActionResult<ProjectTeam>> GetJoinedProjectTeamsById(Guid teamId)
        {
            var userId = Utils.GetUserIdFromHttpContext(HttpContext);
            var result = await _projectTeamServise.GetJoinedProjectTeamById(new Guid(userId!), teamId);
            return Ok(result);
        }

        [HttpPut("{projectTeamId}")]
        public async Task<IActionResult> RemoveMemberFromTeam(Guid projectTeamId, Guid memberId)
        {
            var result = await _projectTeamServise.RemoveMember(projectTeamId, memberId);
            if (result == 1) return NotFound(new ResponseCodeAndMessageModel(10, "Không tìm thấy nhóm!"));
            else if (result == 2) return Ok(new ResponseCodeAndMessageModel(100, "Thành công!"));
            else if (result == 3) return Ok(new ResponseCodeAndMessageModel(17, "Không thể xóa nhóm trưởng khỏi nhóm!"));
            else return BadRequest(new ResponseCodeAndMessageModel(99, "Thất bại!"));
        }

        [HttpPut("{projectTeamId}")]
        public async Task<IActionResult> AddMemberToTeam(Guid projectTeamId, Guid memberId)
        {
            var result = await _projectTeamServise.AddMember(projectTeamId, memberId);
            if (result == 1) return BadRequest(new ResponseCodeAndMessageModel(9, "Có thành viên đã tham gia nhóm khác!"));
            else if (result == 2) return Ok(new ResponseCodeAndMessageModel(100, "Thành công!"));
            else return BadRequest(new ResponseCodeAndMessageModel(99, "Thất bại!"));
        }
    }
}