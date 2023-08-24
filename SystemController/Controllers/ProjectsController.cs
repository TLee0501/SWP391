using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using Service.ProjectService;
using BusinessObjects.RequestModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using BusinessObjects.ResponseModel;

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
        [HttpGet("{projectId}")]
        public async Task<ActionResult<Project>> GetProjectByID(Guid projectId)
        {
            if (projectId == Guid.Empty) return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.GetProjectByID(projectId);
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

        /*[HttpGet("{classId}")]
        public async Task<ActionResult<Project>> GetProjectsByClassID(Guid classId)
        {
            if (classId == Guid.Empty || classId == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.GetProjectsByClassID(classId);
            if (result == null) return BadRequest("Không tìm thấy Project!");
            return Ok(result);
        }*/

        [HttpGet("{classId}"), Authorize]
        public async Task<ActionResult<ProjectAndStatusResponse>> GetProjectsAndStatusByClassIDandUserID(Guid classId)
        {
            var roleClaim = User?.FindAll(ClaimTypes.Name);
            var userID = new Guid(roleClaim?.Select(c => c.Value).SingleOrDefault().ToString());

            if (classId == Guid.Empty) return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.GetProjectsAndStatusByClassIDandUserID(classId, userID);
            if (result.IsNullOrEmpty()) return BadRequest("Không tìm thấy Project!");
            return Ok(result);
        }

        /*[HttpGet("{classId}")]
        public async Task<ActionResult<Project>> SearchProjectInClass(Guid classId, string? searchName)
        {
            if (classId == Guid.Empty || classId == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.SearchProjectInClass(classId, searchName);
            if (result == null || result.Count == 0) return BadRequest("Không tìm thấy Project!");
            return Ok(result);
        }*/

        [HttpGet("{classId}"), Authorize]
        public async Task<ActionResult<Project>> GetProjectsByFilter(Guid classId, string? searchName, bool hasUserId)
        {
            var roleClaim = User?.FindAll(ClaimTypes.Name);
            var userID = new Guid(roleClaim?.Select(c => c.Value).SingleOrDefault().ToString());

            if (classId == Guid.Empty || classId == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _projectService.GetProjectsByFilter(classId, userID, searchName, hasUserId);
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

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            try
            {
                var result = await _projectService.DeleteProject(projectId);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Không tìm thấy dự án.");
                else return Ok("Thành công!");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công.");
            }
        }

        [HttpGet("{classId}"), Authorize]
        public async Task<ActionResult<Project>> GetAllProjects(Guid classId, string? searchName)
        {
            var result = await _projectService.GetAllProjectsInClass(classId, searchName);
            return Ok(result);
        }

        [HttpGet("{classId}"), Authorize]
        public async Task<ActionResult<Project>> GetWorkingProjects(Guid classId)
        {
            var userId = Utils.GetUserIdFromHttpContext(HttpContext);
            var result = await _projectService.GetWorkingProjectsInClass(new Guid(userId!), classId);
            return Ok(result);
        }
    }
}
