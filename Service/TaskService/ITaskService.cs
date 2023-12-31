﻿using BusinessObjects.Enums;
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
        Task<int> CreateTask(Guid userId, CreateTaskRequest request);
        Task<int> UpdateTask(UpdateTaskRequest request);
        Task<int> DeleteTask(Guid taskId);
        Task<int> AssignTask(AssignTaskRequest request);
        Task<int> UnAssignTask(AssignTaskRequest request);
        Task<List<TaskResponse>> GetAllTask(Guid projectId);
        Task<bool> UpdateTaskStatus(Guid taskId, ProjectTaskStatus status);
    }
}
