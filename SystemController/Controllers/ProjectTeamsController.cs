using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Service.ProjectTeamService;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<TeamRequestResponse>>> GetTeamProjectRequests(Guid classId)
        {
            var userId = Utils.GetUserIdFromHttpContext(HttpContext);
            var role = Utils.GetUserRoleFromHttpContext(HttpContext);
            List<TeamRequestResponse>? result;
            if (role == "Student")
            {
                result = await _projectTeamServise.GetTeamProjectRequests(new Guid(userId!), classId);
            }
            else
            {
                result = await _projectTeamServise.GetTeamProjectRequests(null, classId);
            }

            return result;
        }

        // GET: api/ProjectTeams/5
        [HttpGet("{ProjectTeamId}")]
        public async Task<ActionResult<ProjectTeam>> GetProjectTeamById(Guid ProjectTeamId)
        {
            var result = await _projectTeamServise.getProjectTeamById(ProjectTeamId);
            if (result == null) return NotFound("Không tìm thấy ProjectTeam!");
            return Ok(result);
        }

        // PUT: api/ProjectTeams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{teamId}")]
        public async Task<IActionResult> DenyTeamProjectrequest(Guid teamId)
        {
            if (teamId == Guid.Empty)
            {
                return BadRequest("Không nhận được teamId!");
            }

            var result = await _projectTeamServise.DenyTeamProjectrequest(teamId);
            if (result == 1) return BadRequest("Không tìm thấy yêu cầu!");
            else if (result == 2) return BadRequest("Yêu cầu đã bị từ chối!");
            else if (result == 3) return Ok("Thành công!");
            else return BadRequest("Thất bại!");
        }

        [HttpPut("{teamId}")]
        public async Task<IActionResult> CancelTeamProjectrequest(Guid teamId)
        {
            if (teamId == Guid.Empty)
            {
                return BadRequest("Không nhận được teamId!");
            }

            var result = await _projectTeamServise.CancelProjectrequest(teamId);
            if (result == 1) return BadRequest("Không tìm thấy yêu cầu!");
            else if (result == 2) return BadRequest("Yêu cầu đã bị từ chối!");
            else if (result == 3) return Ok("Thành công!");
            else return BadRequest("Thất bại!");
        }

        [HttpPut("{teamId}")]
        public async Task<IActionResult> AcceptTeamProjectrequest(Guid teamId)
        {
            if (teamId == Guid.Empty)
            {
                return BadRequest("Không nhận được teamId!");
            }

            var result = await _projectTeamServise.AcceptTeamProjectrequest(teamId);
            if (result == 1) return BadRequest("Không tìm thấy yêu cầu!");
            else if (result == 2) return BadRequest("Yêu cầu đã bị chấp nhận!");
            else if (result == 3) return Ok("Thành công!");
            else if (result == 4) return BadRequest("Có thành viên đã tham gia vào nhóm khác!");
            else if (result == 5) return BadRequest("Dự án đã được chọn!");
            else return BadRequest("Thất bại!");
        }

        // POST: api/ProjectTeams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> StudentCreateTeamRequest(StudentCreateTeamRequest request)
        {
            if (request.ProjectId == Guid.Empty) return BadRequest("Chưa có ID đề tài!");
            try
            {
                var result = await _projectTeamServise.StudentCreateTeamRequest(request);
                if (result == 1) return BadRequest("Không tìm thấy lớp học!");
                else if (result == 2) return Ok("Thành công!");
                else if (result == 3) return BadRequest("Tên nhóm đã tồn tại!");
                else return BadRequest("Thất bại!"); //0
            }
            catch (DbUpdateException)
            {
                return BadRequest("Thất bại!");
            }
        }

        [HttpGet("{classId}")]
        public async Task<ActionResult<ProjectTeam>> getProjectTeamInClass(Guid classId)
        {
            var result = await _projectTeamServise.getProjectTeamInClass(classId);
            if (result.IsNullOrEmpty()) return NotFound("Không tìm thấy ProjectTeam!");
            return Ok(result);
        }

        // DELETE: api/ProjectTeams/5
        [HttpDelete("{projectTeamId}")]
        public async Task<IActionResult> DeleteProjectTeam(Guid projectTeamId)
        {
            var result = await _projectTeamServise.DeleteProjectTeam(projectTeamId);
            if (result == 0) return BadRequest("Không tìm thấy nhóm!");
            else if (result == 2) return Ok("Thành công!");
            else return BadRequest("Thất bại!");
        }
    }
}
