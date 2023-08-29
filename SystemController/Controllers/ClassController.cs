using BusinessObjects;
using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.ClassService;

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
        [HttpPut]
        public async Task<ActionResult> UpdateClass(UpdateClassRequest request)
        {
            var result = await _classService.UpdateClass(request);
            if (result == 1) return BadRequest("Lớp học không tồn tại.");
            else if (result == 0) return BadRequest("Thất bại.");
            else { return Ok("Cập nhật lớp học thành công."); }
        }

        [HttpGet("{classId}"), Authorize]
        public async Task<ActionResult<ClassDetailResponse>> GetClassByID(Guid classId)
        {
            var userId = new Guid(Utils.GetUserIdFromHttpContext(HttpContext)!);
            var result = await _classService.GetClassByID(classId, userId);
            return Ok(result);
        }

        [HttpDelete("{classId}")]
        public async Task<IActionResult> DeleteClass(Guid classId)
        {
            if (classId == null) return BadRequest("Không nhận được dữ liệu.");
            try
            {
                var result = await _classService.DeleteClass(classId);
                if (result == 0) return BadRequest("Không thành công.");
                else if (result == 1) return BadRequest("Không tìm thấy lớp học.");
                else return Ok("Thành công!");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công.");
            }
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ClassListResponse>>> SearchClass(Guid? semesterId, Guid? courseId, string? searchText)
        {
            var userId = Utils.GetUserIdFromHttpContext(HttpContext);
            var userGuid = new Guid(userId!);

            var result = await _classService.GetClasses(userGuid, semesterId, courseId, searchText);
            return result;
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<string>> EnrollClass(EnrollClassRequest request)
        {
            var role = Utils.GetUserRoleFromHttpContext(HttpContext);

            if (!role!.Equals(Roles.STUDENT))
            {
                return Forbid();
            }

            var userId = Utils.GetUserIdFromHttpContext(HttpContext);
            var userGuid = new Guid(userId!);

            var success = await _classService.EnrollClass(userGuid, request.ClassId, request.EnrollCode);
            if (success)
            {
                return Ok("Tham gia lớp học thành công.");
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserListResponse>>> GetUsersInClass(Guid classId)
        {
            var result = await _classService.GetUsersInClass(classId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserListResponse>>> GetStudentsNotInProjectInClass(Guid classId)
        {
            var result = await _classService.GetStudentsNotInProjectInClass(classId);
            return Ok(result);
        }

        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<ClassListResponse>>> GetAllClasses(Guid? semesterId, Guid? courseId, string? searchText)
        {
            var userId = Utils.GetUserIdFromHttpContext(HttpContext);
            var userGuid = new Guid(userId!);

            var result = await _classService.GetClasses(userGuid, semesterId, courseId, searchText);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> AssignClass(AssignClassRequest request)
        {
            var result = await _classService.AssignClass(request);
            if (request == null) return BadRequest(new ResponseCodeAndMessageModel(2, "Không tìm thấy dữ liệu."));
            else if (result == 1) return BadRequest(new ResponseCodeAndMessageModel(1, "Trùng lớp học."));
            else if (result == 2) return BadRequest(new ResponseCodeAndMessageModel(100, "Thành công."));
            else if (result == 3) return BadRequest(new ResponseCodeAndMessageModel(3, "Không tìm thấy lớp."));
            else if (result == 4) return BadRequest(new ResponseCodeAndMessageModel(4, "Không tìm thấy giảng viên."));
            else return BadRequest(new ResponseCodeAndMessageModel(99, "Không thành công."));
        }

        [HttpPost]
        public async Task<ActionResult> UnassignClass(AssignClassRequest request)
        {
            if (request == null) return BadRequest(new ResponseCodeAndMessageModel(2, "Không tìm thấy dữ liệu."));
            try
            {
                var result = await _classService.UnassignClass(request);
                if (result == 0) return BadRequest("Không thành công.");
                else if (result == 1) return BadRequest("Giảng viên đã được bỏ khỏi lớp học.");
                else return Ok("Thành công!");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công.");
            }
        }
        [HttpGet, Authorize]
        public async Task<ActionResult> GetTeacherClassList(Guid teacherId)
        {
            var result = await _classService.GetTeacherClassList(teacherId);
            if (teacherId.Equals(0)) return BadRequest(new ResponseCodeAndMessageModel(2, "Thông tin giảng viên trống."));
            if (result == null)
            {
                return BadRequest(new ResponseCodeAndMessageModel(1, "Không có lớp học nào."));
            }
            else return Ok(result);
        }
    }
}
