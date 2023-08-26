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

        // GET: api/ProjectTeams/5
        [HttpGet("{ProjectTeamId}")]
        public async Task<ActionResult<ProjectTeam>> GetProjectTeamById(Guid ProjectTeamId)
        {
            var result = await _projectTeamServise.getProjectTeamById(ProjectTeamId);
            if (result == null) return NotFound("Không tìm thấy ProjectTeam!");
            return Ok(result);
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
