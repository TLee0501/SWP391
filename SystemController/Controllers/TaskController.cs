using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.TaskService;
using System.Data.Common;

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly Swp391onGoingReportContext _context;
        private readonly ITaskService _taskService;

        public TaskController(Swp391onGoingReportContext context, ITaskService taskService)
        {
            _context = context;
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTask(CreateTaskRequest request)
        {
            if (request == null)
            {
                return BadRequest("Không nhận được dữ liệu.");

            }
            try
            {
                var result = await _taskService.CreateTask(request);
                if (result == 0) return BadRequest("Không thành công.");
                else if (result == 1) return BadRequest("Task đã tồn tại.");
                else return Ok("Task đã được tạo thành công.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công.");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTask(UpdateTaskRequest request)
        {
            var result = await _taskService.UpdateTask(request);
            if (result == 1) return BadRequest("Task không tồn tại.");
            else if (result == 0) return BadRequest("Thất bại.");
            else { return Ok("Cập nhật task thành công."); }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteTask(Guid taskId)
        {
            if (taskId == null) return BadRequest("Không nhận được dữ liệu.");
            try
            {
                var result = await _taskService.DeleteTask(taskId);
                if (result == 0) return BadRequest("Không thành công.");
                else if (result == 1) return BadRequest("Không tìm thấy task.");
                else return Ok("Task đã được xoá thành công.");
            }
            catch (DbException e)
            {
                return BadRequest("Không thành công.");
            }
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskResponse>>> GetAllTask(Guid projectId)
        {
            var check = await _taskService.GetAllTask(projectId);
            if (check.IsNullOrEmpty()) return BadRequest("Không tìm thấy task.");
            return check;
        }
    }
}
