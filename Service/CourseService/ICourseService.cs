using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;

namespace Service.CourseService
{
    public interface ICourseService
    {
        Task<int> CreateCourse(Guid userId, CourseCreateRequest request);
        Task<List<CourseResponse>> GetCourseForTeacher(Guid teacherId);
        Task<CourseResponse> GetCourseByID(Guid courseId);
        Task<int> AssignCourseToTeacher(AssignCourseRequest request);
        Task<int> UnassignCourseToTeacher(AssignCourseRequest request);
        Task<int> ActiveCourse(Guid courseId);
        Task<int> DeactiveCourse(Guid courseId);
        Task<int> DeleteCourse(Guid courseId);
        Task<List<CourseResponse>> SearchCourse(string? searchText);
        Task<int> UpdateCourse(CoursceUpdateRequest request);
    }
}
