using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = BusinessObjects.Models.Task;

namespace Service.TaskService
{
    public class TaskService : ITaskService
    {
        private readonly Swp391onGoingReportContext _context;
        public TaskService(Swp391onGoingReportContext context)
        {
            _context = context;
        }
        public async Task<int> CreateTask(Guid userId, CreateTaskRequest request)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(x => x.TaskName.ToLower() == request.TaskName.ToLower() && x.IsDeleted == false);
            if (task != null)
            {
                return 1;
            }
            var id = Guid.NewGuid();
            var newTask = new Task
            {
                TaskId = id,
                UserId = userId,
                ProjectId = request.ProjectId,
                TaskName = request.TaskName,
                Description = request.TaskDescription,
                Status = 0,
                IsDeleted = false,
            };
            // status = 0: chưa làm
            //status = 1: đang làm
            //status = 2: đã hoàn thành
            try
            {
                await _context.AddAsync(newTask);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (DbException e)
            {
                return 0;
            }
        }

        public async Task<int> UpdateTask(UpdateTaskRequest request)
        {
            var check = await _context.Tasks.FindAsync(request.TaskId);
            if (check == null)
            {
                return 1;
            }
            check.TaskName = request.TaskName;
            check.Description = request.Description;
            check.Status = request.Status;
            try
            {
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (DbException e)
            {
                return 0;
            }
        }


        public async Task<int> DeleteTask(Guid taskId)
        {
            var check = await _context.Tasks.FindAsync(taskId);
            if (check == null)
            {
                return 1;
            }
            try
            {
                check.IsDeleted = true;
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<List<TaskResponse>> GetAllTask(Guid projectId)
        {
            var check = await _context.Tasks.Where(x => x.ProjectId == projectId && x.IsDeleted == false).ToListAsync();
            if (check == null || check.Count == 0)
            {
                return null;
            }
            var list = new List<TaskResponse>();
            foreach (var item in check)
            {
                var fullName = await _context.Users.FindAsync(item.UserId);
                var tmp = new TaskResponse
                {
                    ProjectId = item.ProjectId,
                    UserId = item.UserId,
                    TaskId = item.TaskId,
                    TaskName = item.TaskName,
                    TaskDescription = item.Description,
                    Status = item.Status,
                    UserFullName = fullName.FullName,
                };
                list.Add(tmp);
            }
            return list;
        }
    }
}
