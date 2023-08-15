using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Service.ProjectTeamService;

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

        // PUT: api/ProjectTeams/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutProjectTeam(Guid id, ProjectTeam projectTeam)
        {
            if (id != projectTeam.ProjectTeamId)
            {
                return BadRequest();
            }

            _context.Entry(projectTeam).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTeamExists(id))
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

        // POST: api/ProjectTeams
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost]
        public async Task<ActionResult<ProjectTeam>> PostProjectTeam(ProjectTeam projectTeam)
        {
          if (_context.ProjectTeams == null)
          {
              return Problem("Entity set 'Swp391onGoingReportContext.ProjectTeams'  is null.");
          }
            _context.ProjectTeams.Add(projectTeam);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjectTeamExists(projectTeam.ProjectTeamId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProjectTeam", new { id = projectTeam.ProjectTeamId }, projectTeam);
        }*/

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
