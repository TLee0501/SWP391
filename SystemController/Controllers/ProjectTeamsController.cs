using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Service.ProjectTeamService;
using BusinessObjects.RequestModel;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using BusinessObjects.ResponseModel;
using Microsoft.IdentityModel.Tokens;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamRequestResponse>>> GetTeamProjectRequests(Guid classId)
        {
            var result = await _projectTeamServise.GetTeamProjectRequests(classId);
            if (result == null) return BadRequest("Không tìm thấy Request!");
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
        [HttpPut("{id}")]
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

        // POST: api/ProjectTeams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> StudentCreateTeamRequest(StudentCreateTeamRequest request)
        {
            if (request.TeamName.IsNullOrEmpty()) return BadRequest("Chưa có tên nhóm!");
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

        // DELETE: api/ProjectTeams/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectTeam(Guid id)
        {
            if (_context.ProjectTeams == null)
            {
                return NotFound();
            }
            var projectTeam = await _context.ProjectTeams.FindAsync(id);
            if (projectTeam == null)
            {
                return NotFound();
            }

            _context.ProjectTeams.Remove(projectTeam);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/
    }
}
