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
        Task<int> CreateCourse(CourseCreateRequest request);
        Task<List<CourseResponse>> GetCourseForTeacher(Guid teacherID);
        Task<CourseResponse> GetCourseByID(Guid courseID);
        Task<int> AssignCourseToTeacher(AssignCourseRequest request);
        Task<int> UnassignCourseToTeacher(AssignCourseRequest request);
        Task<int> ActiveCourse(Guid courseID);
        Task<int> DeactiveCourse(Guid courseID);
        Task<int> DeleteCourse(Guid courseID);
    }
}
