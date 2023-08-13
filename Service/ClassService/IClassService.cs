using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ClassService
{
    public interface IClassService
    {
        Task<int> CreateClass(CreateClassRequest request);
        Task<ClassResponse> GetClassByID(Guid classID);
        Task<int> DeleteClass(Guid classID);
        Task<List<ClassResponse>> SearchClass(Guid courseID, string searchText);
    }
}
