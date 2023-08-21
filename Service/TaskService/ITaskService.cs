using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.TaskService
{
    public interface ITaskService
    {
        Task<int> CreateTask(CreateTaskRequest request);
        Task<int> UpdateTask(UpdateTaskRequest request);
        Task<int> DeleteTask(Guid taskId);
        Task<List<TaskResponse>> GetAllTask(Guid projectId);
    }
}
