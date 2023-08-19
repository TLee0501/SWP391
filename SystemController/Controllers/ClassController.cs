using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.ClassService;
using Service.CourseService;
using System.Data.Common;
using System.Security.Claims;
using System.Security.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly Swp391onGoingReportContext _context;
        private readonly IClassService _classService;
        public ClassController(Swp391onGoingReportContext context, IClassService classService)
        {
            _context = context;
            _classService = classService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClassforTest()
        {
            if (_context.Classes == null)
            {
                return NotFound();
            }
            return await _context.Classes.ToListAsync();
        }

        // GET api/<ClassController>/5
        [HttpPost]
        public async Task<ActionResult> CreateClass(CreateClassRequest request)
        {
            if (request == null) return BadRequest("Không nhận được dữ liệu.");
            try
            {
                var result = await _classService.CreateClass(request);
                if (result == 0) return BadRequest("Không thành công.");
                else if (result == 1) return BadRequest("Lớp đã tồn tại.");
                else return Ok("Lớp học đã được tạo thành công.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công.");
            }
        }

        [HttpGet("{classId}")]
        public async Task<ActionResult<ClassResponse>> GetClassByID(Guid classId)
        {
            if (classId == null) return BadRequest("Không nhận được dữ liệu.");
            var result = await _classService.GetClassByID(classId);

            if (result == null)
            {
                return NotFound("Không tìm thấy.");
            }

            return result;
        }

        [HttpDelete("{classId}")]
        public async Task<IActionResult> DeleteClass(Guid classId)
        {
            if (classId == null) return BadRequest("Không nhận được dữ liệu.");
            try
            {
                var result = await _classService.DeleteClass(classId);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Không tìm thấy lớp học.");
                else return Ok("Thành công!");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công.");
            }
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ClassResponse>>> SearchClass(Guid? courseId = null, string? searchText = null)
        {
            var role = Utils.GetUserRoleFromHttpContext(HttpContext);
            var userId = Utils.GetUserIdFromHttpContext(HttpContext);
            var userGuid = new Guid(userId!);

            var result = await _classService.GetClasses(userGuid, role, courseId, searchText);
            return result;
        }
    }
}
