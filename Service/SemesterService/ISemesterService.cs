using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.SemesterService
{
    public interface ISemesterService
    {
        Task<int> CreateSemester(SemesterCreateRequest request);
        Task<List<SemesterResponse>> GetSemesterList();
        Task<SemesterDetailResponse?> GetSemester(Guid semesterId);
        Task<int> UpdateSemester(Guid semesterId, SemesterCreateRequest request);
        //Task<SemesterTypeResponse> GetSemesterType(Guid semesterTypeId);
        //Task<List<SemesterTypeResponse>> GetSemesterTypes();
    }
}
