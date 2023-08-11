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
        [HttpGet("{teacherID}")]
        public async Task<ActionResult<IEnumerable<CourseResponse>>> GetCourseForTeacher(Guid teacherID)
        {
            if (teacherID == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _courseService.GetCourseForTeacher(teacherID);

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

        [HttpPut("{courseID}")]
        public async Task<IActionResult> ActiveCourse(Guid courseID)
        {
            try
            {
                var result = await _courseService.ActiveCourse(courseID);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Khóa học đã kích hoạt!");
                else if (result == 3) return BadRequest("Không tìm thấy gói!");
                else return Ok("Kích hoạt thành công!");
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Không thành công!");
            }
        }

        [HttpPut("{courseID}")]
        public async Task<IActionResult> DeactiveCourse(Guid courseID)
        {
            try
            {
                var result = await _courseService.DeactiveCourse(courseID);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Khóa học đã hủy kích hoạt!");
                else if (result == 3) return BadRequest("Không tìm thấy gói!");
                else return Ok("Hủy kích hoạt thành công!");
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest("Không thành công!");
            }
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> CreateCourse(CourseCreateRequest request)
        {
            if (request == null) return BadRequest("Không nhận được dữ liệu!");
            try
            {
                var result = await _courseService.CreateCourse(request);
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
        [HttpDelete("{courseID}")]
        public async Task<IActionResult> DeleteCourse(Guid courseID)
        {
            if (courseID == null) return BadRequest("Không nhận được dữ liệu!");
            try
            {
                var result = await _courseService.DeleteCourse(courseID);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Không tìm thấy khóa học đã tồn tại!");
                else return Ok("Thành công!");  //2
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công!");
            }
        }

        [HttpGet("{courseID}")]
        public async Task<ActionResult<CourseResponse>> GetCourseByID(Guid courseID)
        {
            if (courseID == null) return BadRequest("Không nhận được dữ liệu!");
            var result = await _courseService.GetCourseByID(courseID);

            if (result == null)
            {
                return NotFound("Không tìm thấy!");
            }

            return result;
        }
    }
}
