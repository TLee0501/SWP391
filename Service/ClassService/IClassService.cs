﻿using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.ClassService
{
    public interface IClassService
    {
        Task<int> CreateClass(CreateClassRequest request);
        Task<ClassResponse?> GetClassByID(Guid classId, Guid? userId, string? role);
        Task<int> DeleteClass(Guid classId);
        Task<List<ClassResponse>> GetClasses(Guid userID, string? role, Guid? courseID, string? searchText);
        Task<bool> EnrollClass(Guid userId, Guid classId, string enrollCode);
        Task<List<UserListResponse>> GetUsersInClass(Guid classId);
    }
}
