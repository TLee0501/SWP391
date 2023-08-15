using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        Task<List<CourseResponse>> SearchCourse(string searchText);
    }
}
