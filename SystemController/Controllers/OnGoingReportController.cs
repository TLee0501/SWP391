using BusinessObjects.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.OnGoingReportService;

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OnGoingReportController : ControllerBase
    {
        private readonly IOnGoingReportService _onGoingReportService;

        public OnGoingReportController(IOnGoingReportService onGoingReportService)
        {
            _onGoingReportService = onGoingReportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportTaskResponse>>> GetOnGoingReportInClass(Guid classId)
        {
            var result = await _onGoingReportService.GetOnGoingReportInClass(classId);
            if (result.IsNullOrEmpty()) return NotFound("Không tìm thấy!");
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportTaskResponse>>> GetOnGoingReportInProject(Guid projectId)
        {
            var result = await _onGoingReportService.GetOnGoingReportInProject(projectId);
            if (result == null) return NotFound("Không tìm thấy!");
            return Ok(result);
        }
    }
}
