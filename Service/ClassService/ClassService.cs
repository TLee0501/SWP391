using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ClassService
{
    public class ClassService : IClassService
    {
        private readonly Swp391onGoingReportContext _context;
        public ClassService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<int> CreateClass(CreateClassRequest request)
        {
            var classes = await _context.Classes.SingleOrDefaultAsync(x => x.ClassName.ToLower() == request.ClassName.ToLower() && x.IsDeleted == false);
            if (classes != null) return 1;

            var id = Guid.NewGuid();
            var newclass = new Class
            {
                ClassId = id,
                UserId = request.UserId,
                CourseId = request.CourseId,
                ClassName = request.ClassName,
                TimeStart = request.TimeStart,
                TimeEnd = request.TimeEnd,
                IsDeleted = false,
                IsCompleted = false,
            };
            try
            {
                await _context.Classes.AddAsync(newclass);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DeleteClass(Guid classId)
        {
            var check = await _context.Classes.FindAsync(classId);
            if (check == null) return 1;
            try
            {
                check.IsDeleted = true;
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<ClassResponse> GetClassByID(Guid classId)
        {
            var classes = await _context.Classes.FindAsync(classId);
            if (classes == null) return null;

            var user = await _context.Users.FindAsync(classes.UserId);
            var course = await _context.Courses.FindAsync(classes.CourseId);
            var classResponse = new ClassResponse();
            if (classes.TimeEnd == null)
            {
                classResponse = new ClassResponse
                {
                    ClassId = classId,
                    ClassName = classes.ClassName,
                    UserId = classes.UserId,
                    CourseCode = course.CourseCode,
                    StartTime = classes.TimeStart
                };
            }
            else
            {
                classResponse = new ClassResponse
                {
                    ClassId = classId,
                    ClassName = classes.ClassName,
                    UserId = classes.UserId,
                    CourseCode = course.CourseCode,
                    StartTime = classes.TimeStart,
                    EndTime = classes.TimeEnd
                };
            }
            return classResponse;
        }

        public async Task<List<ClassResponse>> SearchClass(Guid courseId, string searchText)
        {
            var result = new List<ClassResponse>();
            var classes = await _context.Classes.Where(x => x.IsDeleted == false).ToListAsync();
            if (classes == null || classes.Count == 0) return null;
            var course = await _context.Courses.FindAsync(courseId);
            foreach (var item in classes)
            {
                if (item.ClassName.ToLower().Contains(searchText.ToLower()))
                {
                    var tmp = new ClassResponse();
                    if (item.TimeEnd == null)
                    {
                        tmp = new ClassResponse
                        {
                            ClassId = item.ClassId,
                            ClassName = item.ClassName,
                            CourseCode = course.CourseCode,
                            UserId = item.UserId,
                            StartTime = item.TimeStart
                        };
                    }
                    else
                    {
                        tmp = new ClassResponse
                        {
                            ClassId = item.ClassId,
                            ClassName = item.ClassName,
                            CourseCode = course.CourseCode,
                            UserId = item.UserId,
                            StartTime = item.TimeStart,
                            EndTime = (DateTime)item.TimeEnd,
                        };
                    }
                    result.Add(tmp);
                }
            }
            return result;
        }
    }
}
