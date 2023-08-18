using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ClassService
{
    public interface IClassService
    {
        Task<int> CreateClass(CreateClassRequest request);
        Task<ClassResponse> GetClassByID(Guid classId);
        Task<int> DeleteClass(Guid classId);
        Task<List<ClassResponse>> GetClasses(Guid? courseID, string? searchText);
    }
}
