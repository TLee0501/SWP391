using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using Service.CourseService;
using BusinessObjects.ResponseModel;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly Swp391onGoingReportContext _context;
        private readonly ICourseService _courseService;

        public CoursesController(Swp391onGoingReportContext context, ICourseService courseService)
        {
            _context = context;
            _courseService = courseService;
        }


        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCoursesForTest()
        {
          if (_context.Courses == null)
          {
              return NotFound();
          }
            return await _context.Courses.ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet , Authorize]
        public async Task<ActionResult<IEnumerable<CourseResponse>>> GetCourseForTeacher()
        {
            var roleClaim = User?.FindAll(ClaimTypes.Name);
            var teacherID = new Guid(roleClaim?.Select(c => c.Value).SingleOrDefault().ToString());

            if (teacherID == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _courseService.GetCourseForTeacher(teacherID);

            if (result == null || result.Count == 0)
            {
                return NotFound("Không tìm thấy!");
            }

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseResponse>>> SearchCourse(string searchText)
        {
            if (searchText == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _courseService.SearchCourse(searchText);

            if (result == null || result.Count == 0)
            {
                return NotFound("Không tìm thấy!");
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult> AssignCourseToTeacher(AssignCourseRequest request)
        {
            if (request == null) return BadRequest("Không nhận được dữ liệu!");
            try
            {
                var result = await _courseService.AssignCourseToTeacher(request);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Giảng viên chưa được thêm vào khóa học!");
                else return Ok("Thêm thành công!");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công!");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UnassignCourseToTeacher(AssignCourseRequest request)
        {
            if (request == null) return BadRequest("Không nhận được dữ liệu!");
            try
            {
                var result = await _courseService.UnassignCourseToTeacher(request);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Giảng viên đã được bỏ khỏi khóa học!");
                else return Ok("Thành công!");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công!");
            }
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> ActiveCourse(Guid courseId)
        {
            try
            {
                var result = await _courseService.ActiveCourse(courseId);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Khóa học đã kích hoạt!");
                else if (result == 3) return BadRequest("Không tìm thấy khoá!");
                else return Ok("Kích hoạt thành công!");
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Không thành công!");
            }
        }

        [HttpPut("{courseId}")]
        public async Task<IActionResult> DeactiveCourse(Guid courseId)
        {
            try
            {
                var result = await _courseService.DeactiveCourse(courseId);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Khóa học đã hủy kích hoạt!");
                else if (result == 3) return BadRequest("Không tìm thấy khoá!");
                else return Ok("Hủy kích hoạt thành công!");
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Không thành công!");
            }
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult> CreateCourse(CourseCreateRequest request)
        {
            var roleClaim = User?.FindAll(ClaimTypes.Name);
            var userID = new Guid(roleClaim?.Select(c => c.Value).SingleOrDefault().ToString());

            if (request == null) return BadRequest("Không nhận được dữ liệu!");
            try
            {
                var result = await _courseService.CreateCourse(userID, request);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Tên khóa học đã tồn tại!");
                else return Ok("Tạo thành công!");  //2
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công!");
            }
        }

        // DELETE: api/Courses/5
        [HttpDelete("{courseId}")]
        public async Task<IActionResult> DeleteCourse(Guid courseId)
        {
            if (courseId == null) return BadRequest("Không nhận được dữ liệu!");
            try
            {
                var result = await _courseService.DeleteCourse(courseId);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Không tìm thấy khóa học đã tồn tại!");
                else return Ok("Thành công!");  //2
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công!");
            }
        }

        [HttpGet("{courseId}")]
        public async Task<ActionResult<CourseResponse>> GetCourseByID(Guid courseId)
        {
            if (courseId == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _courseService.GetCourseByID(courseId);

            if (result == null)
            {
                return NotFound("Không tìm thấy!");
            }

            return result;
        }
    }
}
