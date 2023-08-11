using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Service.ProjectService;

namespace SystemController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly Swp391onGoingReportContext _context;
        private readonly IProjectService _projectService;

        public ProjectsController(Swp391onGoingReportContext context, IProjectService projectService)
        {
            _context = context;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsForTest()
        {
          if (_context.Projects == null)
          {
              return NotFound();
          }
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Projects/5
        /*[HttpGet("{projectID}")]
        public async Task<ActionResult<Project>> GetProjectByID(Guid projectID)
        {
            if(projectID == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.GetProjectByID(projectID);
            if (result == null) return BadRequest("Không tùm thấy Project!");
            return Ok(result);
        }*/

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProject(Guid id, Project project)
        {
            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Projects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
          if (_context.Projects == null)
          {
              return Problem("Entity set 'Swp391onGoingReportContext.Projects'  is null.");
          }
            _context.Projects.Add(project);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProjectExists(project.ProjectId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProject", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Projects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            if (_context.Projects == null)
            {
                return NotFound();
            }
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(Guid id)
        {
            return (_context.Projects?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }
    }
}
