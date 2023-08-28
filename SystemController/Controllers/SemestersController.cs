using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.SemesterService;

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SemestersController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemestersController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }


        // GET: api/Semesters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SemesterResponse>>> GetSemesterList()
        {
            var result = await _semesterService.GetSemesterList();
            if (result.IsNullOrEmpty()) return NotFound(new ResponseCodeAndMessageModel(6, "Không tìm thấy học kỳ!"));
            return Ok(result);
        }

        // GET: api/Semesters/5
        [HttpGet("{semesterId}")]
        public async Task<ActionResult<SemesterResponse>> GetSemester(Guid semesterId)
        {
            var result = await _semesterService.GetSemester(semesterId);
            if (result == null) return NotFound(new ResponseCodeAndMessageModel(6, "Không tìm thấy học kỳ!"));
            return Ok(result);
        }

        // PUT: api/Semesters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{semesterId}")]
        public async Task<IActionResult> UpdateSemester(Guid semesterId, SemesterCreateRequest request)
        {
            var result = await _semesterService.UpdateSemester(semesterId, request);
            if (result.Equals(1))
                return BadRequest(new ResponseCodeAndMessageModel(6, "Không tìm thấy học kỳ!"));
            else if (result.Equals(2))
                return BadRequest(new ResponseCodeAndMessageModel(1, "Tên học kỳ bị trùng!"));
            else if (result.Equals(3))
                return BadRequest(new ResponseCodeAndMessageModel(2, "Ngày bắt đầu bị trùng!"));
            else if (result.Equals(4))
                return BadRequest(new ResponseCodeAndMessageModel(3, "Ngày kết thúc bị trùng!"));
            else if (result.Equals(5))
                return Ok(new ResponseCodeAndMessageModel(100, "Thành công!"));
            else
                return BadRequest(new ResponseCodeAndMessageModel(99, "Thất bại!"));
        }

        // POST: api/Semesters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Semester>> CreateSemester(SemesterCreateRequest request)
        {
            if (request.StartTime.Date <= DateTime.Now.Date)
                return BadRequest(new ResponseCodeAndMessageModel(4, "Ngày bắt đầu nhỏ hơn hiện tại"));
            if (request.EndTime.Date <= request.StartTime.Date)
                return BadRequest(new ResponseCodeAndMessageModel(5, "Ngày bắt đầu lớn hơn ngày kết thúc!"));

            var result = await _semesterService.CreateSemester(request);
            if (result.Equals(1))
                return BadRequest(new ResponseCodeAndMessageModel(1, "Tên học kỳ bị trùng!"));
            else if (result.Equals(2))
                return BadRequest(new ResponseCodeAndMessageModel(2, "Ngày bắt đầu bị trùng!"));
            else if (result.Equals(3))
                return BadRequest(new ResponseCodeAndMessageModel(3, "Ngày kết thúc bị trùng!"));
            else if (result.Equals(4))
                return Ok(new ResponseCodeAndMessageModel(100, "Thành công!"));
            else
                return BadRequest(new ResponseCodeAndMessageModel(99, "Thất bại!"));
        }

        /*[HttpGet("{semesterTypeId}")]
        public async Task<ActionResult<Semester>> GetSemesterType(Guid semesterTypeId)
        {
            var result = await _semesterService.GetSemesterType(semesterTypeId);
            if (result == null) return NotFound(new ResponseCodeAndMessageModel(16, "Không tìm thấy loại học kỳ!"));
            else
                return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<Semester>> GetSemesterTypes()
        {
            var result = await _semesterService.GetSemesterTypes();
            if (result == null) return NotFound(new ResponseCodeAndMessageModel(16, "Không tìm thấy loại học kỳ!"));
            else
                return Ok(result);
        }*/

        // DELETE: api/Semesters/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemester(Guid id)
        {
            if (_context.Semesters == null)
            {
                return NotFound();
            }
            var semester = await _context.Semesters.FindAsync(id);
            if (semester == null)
            {
                return NotFound();
            }

            _context.Semesters.Remove(semester);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/
    }
}
