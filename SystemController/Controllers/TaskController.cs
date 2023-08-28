using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost, Authorize]
        public async Task<ActionResult> CreateTask(CreateTaskRequest request)
        {
            if (request == null)
            {
                return BadRequest("Không nhận được dữ liệu.");
            }
            try
            {
                var userId = Utils.GetUserIdFromHttpContext(HttpContext);
                var result = await _taskService.CreateTask(new Guid(userId!), request);
                if (result == 0) return BadRequest("Không thành công.");
                else if (result == 2) return BadRequest(new ResponseCodeAndMessageModel(2, "Bạn không phải trưởng nhóm."));
                else if (result == 4) return BadRequest(new ResponseCodeAndMessageModel(2, "Task đã tồn tại."));
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

        [HttpPut]
        public async Task<ActionResult> AssignTask(AssignTaskRequest request)
        {
            if (request == null) return BadRequest("Không nhận được dữ liệu.");
            try
            {
                var result = await _taskService.AssignTask(request);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Sinh viên đã được thêm vào task.");
                else return Ok("Thêm thành công.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công.");
            }
        }

        [HttpPut]
        public async Task<ActionResult> UnAssignTask(AssignTaskRequest request)
        {
            if (request == null) return BadRequest("Không nhận được dữ liệu.");
            try
            {
                var result = await _taskService.UnAssignTask(request);
                if (result == 0) return BadRequest("Không thành công!");
                else if (result == 1) return BadRequest("Sinh viên chưa được thêm vào task.");
                else return Ok("Xóa thành công.");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Không thành công.");
            }
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
            var tasks = await _taskService.GetAllTask(projectId);
            if (tasks == null || tasks.Count == 0)
            {
                return BadRequest(new ResponseCodeAndMessageModel(1, "Không có task nào tồn tại."));
            }
            else return tasks;
        }
    }
}
