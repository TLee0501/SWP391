using BusinessObjects.Enums;
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
        public async Task<ActionResult<SemesterDetailResponse?>> GetSemester(Guid semesterId)
        {
            var result = await _semesterService.GetSemester(semesterId);
            return Ok(result);
        }

        // PUT: api/Semesters/5
        [HttpPut("{semesterId}")]
        public async Task<IActionResult> UpdateSemester(Guid semesterId, SemesterCreateRequest request)
        {
            var result = await _semesterService.UpdateSemester(semesterId, request);
            if (result.Equals(1))
            {
                return BadRequest(new ResponseCodeAndMessageModel
                {
                    Code = (int)ErrorCode.SemesterOverlapTime,
                    Message = "Ngày bắt đầu hoặc kết thúc bị trùng với học kỳ khác"
                });
            }

            if (result.Equals(-1))
            {
                return BadRequest(new ResponseCodeAndMessageModel
                {
                    Code = (int)ErrorCode.Error,
                    Message = "Có lỗi xảy ra"
                });
            }

            return Ok(new ResponseCodeAndMessageModel(100, "Thành công!"));
        }

        // POST: api/Semesters
        [HttpPost]
        public async Task<ActionResult<Semester>> CreateSemester(SemesterCreateRequest request)
        {
            var result = await _semesterService.CreateSemester(request);
            if (result.Equals(1))
            {
                return BadRequest(new ResponseCodeAndMessageModel
                {
                    Code = (int)ErrorCode.SemesterOverlapTime,
                    Message = "Ngày bắt đầu hoặc kết thúc bị trùng với học kỳ khác"
                });
            }

            if (result.Equals(-1))
            {
                return BadRequest(new ResponseCodeAndMessageModel
                {
                    Code = (int)ErrorCode.Error,
                    Message = "Có lỗi xảy ra"
                });
            }

            return Ok(new ResponseCodeAndMessageModel(100, "Thành công!"));
        }
    }
}
