using System.Composition;
using BusinessObjects.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.TeamReportService;

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ITeamReportService _teamReportService;
        public ReportController(ITeamReportService teamReportService)
        {
            _teamReportService = teamReportService;
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> SendReport(CreateTeamReportRequest request)
        {
            var reporterId = Utils.GetUserIdFromHttpContext(HttpContext);
            var result = await _teamReportService.CreateTeamReport(new Guid(reporterId!), request);
            if (result == 1)
            {
                return BadRequest("Nhóm không hợp lệ");
            }
            if (result == 2)
            {
                return BadRequest("Chưa thề gửi báo cáo vào lúc này");
            }

            return Ok("Gửi báo cáo thành công");
        }

        [HttpGet, Authorize]
        public async Task<ActionResult> GetTeamReports(Guid teamId)
        {
            var reports = await _teamReportService.GetTeamReports(teamId);
            return Ok(reports);
        }

        [HttpGet, Authorize]
        public async Task<ActionResult> GetTeamReportById(Guid reportId)
        {
            var report = await _teamReportService.GetTeamReport(reportId);
            return Ok(report);
        }

        [HttpPost, Authorize]
        public async Task<ActionResult> SendReportFeedback(CreateTeamReportFeedback request)
        {
            var success = await _teamReportService.CreateTeamReportFeedback(request);
            if (success)
            {
                return Ok("Thành công");
            }

            return BadRequest("Có lỗi xảy ra");
        }
    }
}
