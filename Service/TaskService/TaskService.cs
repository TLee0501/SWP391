using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Task = BusinessObjects.Models.Task;
using BusinessObjects.Enums;

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
            var startTime = Utils.ConvertUTCToLocalDateTime(request.StartTime);
            var endTime = Utils.ConvertUTCToLocalDateTime(request.EndTime);

            var id = Guid.NewGuid();
            var newTask = new Task
            {
                TaskId = id,
                UserId = userId,
                ProjectId = request.ProjectId,
                TaskName = request.TaskName,
                Description = request?.TaskDescription ?? "",
                StartTime = startTime,
                EndTime = endTime,
                Status = request!.Status,
                IsDeleted = false,
            };

            try
            {
                await _context.AddAsync(newTask);

                foreach (var asignee in request.Assignees)
                {
                    await _context.StudentTasks.AddAsync(new StudentTask
                    {
                        StudentTaskId = Guid.NewGuid(),
                        TaskId = id,
                        UserId = new Guid(asignee)
                    });
                }

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
            try
            {
                var taskId = request.TaskId;

                var check = await _context.Tasks.FindAsync(taskId);
                if (check == null)
                {
                    return 1;
                }
                var startTime = Utils.ConvertUTCToLocalDateTime(request.StartTime);
                var endTime = Utils.ConvertUTCToLocalDateTime(request.EndTime);

                check.TaskName = request.TaskName;
                check.Description = request.Description;
                check.StartTime = request.StartTime;
                check.EndTime = request.EndTime;
                check.Status = request.Status;
                check.StartTime = startTime;
                check.EndTime = endTime;

                // Remove all old student tasks
                var currentStudentTasks = await _context.StudentTasks
                    .Where(x => x.TaskId == taskId)
                    .ToListAsync();
                if (currentStudentTasks != null && currentStudentTasks.Count > 0)
                {
                    _context.StudentTasks.RemoveRange(currentStudentTasks);
                }

                // Set new student tasks
                var studentTasks = new List<StudentTask>();
                foreach (var assignee in request.Assignees)
                {
                    studentTasks.Add(new StudentTask
                    {
                        StudentTaskId = Guid.NewGuid(),
                        TaskId = taskId,
                        UserId = assignee
                    });
                }

                await _context.StudentTasks.AddRangeAsync(studentTasks);

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

        public async Task<int> AssignTask(AssignTaskRequest request)
        {
            var check = await _context.StudentTasks.SingleOrDefaultAsync(x => x.UserId == request.MemberId && x.TaskId == request.TaskId);
            if (check != null) return 1;

            var studentTask = new StudentTask
            {
                StudentTaskId = Guid.NewGuid(),
                UserId = request.MemberId,
                TaskId = request.TaskId,
            };
            try
            {
                await _context.StudentTasks.AddAsync(studentTask);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public async Task<int> UnAssignTask(AssignTaskRequest request)
        {
            var check = await _context.StudentTasks.SingleOrDefaultAsync(x => x.UserId == request.MemberId && x.TaskId == request.TaskId);
            if (check == null) return 1;

            try
            {
                _context.StudentTasks.Remove(check);
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
            var check = await _context.Tasks.Include(_ => _.StudentTasks).ThenInclude(_ => _.User).Where(x => x.ProjectId == projectId && x.IsDeleted == false).ToListAsync();
            var list = new List<TaskResponse>();
            foreach (var item in check)
            {
                var user = await _context.Users.FindAsync(item.UserId);
                var tmp = new TaskResponse
                {
                    ProjectId = item.ProjectId,
                    UserId = item.UserId,
                    TaskId = item.TaskId,
                    TaskName = item.TaskName,
                    TaskDescription = item.Description,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Status = (ProjectTaskStatus)item.Status,
                    UserFullName = user!.FullName,
                    Members = item.StudentTasks.Select(_ => new TaskMemberResponse
                    {
                        MemberId = _.User.UserId,
                        MemberFullName = _.User.FullName,
                    }).ToList(),
                };
                list.Add(tmp);
            }
            return list;
        }


    }
}
