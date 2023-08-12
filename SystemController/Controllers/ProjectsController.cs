using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Service.ProjectService;
using BusinessObjects.RequestModel;

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
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
        [HttpGet("{projectID}")]
        public async Task<ActionResult<Project>> GetProjectByID(Guid projectID)
        {
            if (projectID == Guid.Empty) return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.GetProjectByID(projectID);
            if (result == null) return BadRequest("Không tùm thấy Project!");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProject(ProjectCreateRequest request)
        {
            if (request.ClassId == Guid.Empty) return BadRequest("Không nhận được dữ liệu!");
            if (request.ProjectName == null || request.ProjectName == "") return BadRequest("Không nhận được dữ liệu!");
            if (request.Description == null || request.Description == "") return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.CreateProject(request);
            if (result == 0) return BadRequest("Thất bại!");
            return Ok("Thành công!");
        }

        // PUT: api/Projects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        public async Task<IActionResult> UpdateProject(ProjectUpdateRequest request)
        {
            var result = await _projectService.UpdateProject(request);
            if (result == 1) return BadRequest("Project không tồn tại!");
            else if (result == 0) return BadRequest("Thất bại!");
            else return Ok("Thành công!");
        }

        [HttpGet("{classID}")]
        public async Task<ActionResult<Project>> GetProjectsByClassID(Guid classID)
        {
            if (classID == Guid.Empty || classID == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.GetProjectsByClassID(classID);
            if (result == null) return BadRequest("Không tìm thấy Project!");
            return Ok(result);
        }

        [HttpGet("{classID}")]
        public async Task<ActionResult<Project>> SearchProjectInClass(Guid classID, string searchName)
        {
            if (classID == Guid.Empty || classID == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.SearchProjectInClass(classID, searchName);
            if (result == null || result.Count == 0) return BadRequest("Không tìm thấy Project!");
            return Ok(result);
        }

        // DELETE: api/Projects/5
        /*[HttpDelete("{id}")]
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
        }*/
    }
}
