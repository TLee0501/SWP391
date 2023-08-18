using Azure.Core;
using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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
                EnrollCode = request.EnrollCode,
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
                    CourseName = course.CourseName,
                    EnrollCode = classes.EnrollCode,
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
                    CourseName = course.CourseName,
                    EnrollCode = classes.EnrollCode,
                    StartTime = classes.TimeStart,
                    EndTime = classes.TimeEnd
                };
            }
            return classResponse;
        }

        public async Task<List<ClassResponse>> GetClasses(Guid? courseId = null, string? searchText = null)
        {
            IQueryable<Class> query = _context.Classes.Include(item => item.Course);

            if (courseId != null)
            {
                query = query.Where(item => item.CourseId == courseId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(item => item.ClassName.Contains(searchText));
            }

            var classes = await query.ToListAsync();
            var classResponses = classes.Select(item => new ClassResponse
            {
                ClassId = item.ClassId,
                ClassName = item.ClassName,
                CourseCode = item.Course.CourseCode,
                CourseName = item.Course.CourseName,
                EnrollCode = item.EnrollCode,
                UserId = item.UserId,
                StartTime = item.TimeStart,
                EndTime = item.TimeEnd,
            });

            return classResponses.ToList();
        }
    }
}
