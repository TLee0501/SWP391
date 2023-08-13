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

        public async Task<ClassResponse> GetClassByID(Guid classID)
        {
            var classes = await _context.Classes.FindAsync(classID);
            if (classes == null) return null;

            var user = await _context.Users.FindAsync(classes.UserId);
            var classResponse = new ClassResponse
            {
                ClassId = classID,
                ClassName = classes.ClassName,
                UserId = classes.UserId,
                CourseCode = classes.Course.CourseCode,
                StartTime = classes.TimeStart,
                EndTime = (DateTime)classes.TimeEnd
            };
            return classResponse;
        }

        public async Task<List<ClassResponse>> SearchClass(string searchText)
        {
            var result = new List<ClassResponse>();
            var classes = await _context.Classes.Where(x => x.IsDeleted == false).ToListAsync();
            if (classes == null || classes.Count == 0) return null;

            foreach (var item in classes)
            {
                if (item.ClassName.ToLower().Contains(searchText.ToLower()))
                {
                    var tmp = new ClassResponse
                    {
                        ClassId = item.ClassId,
                        ClassName = item.ClassName,
                        CourseCode = item.Course.CourseCode,
                        UserId = item.UserId,
                        StartTime = item.TimeStart,
                        EndTime = item.TimeEnd,
                    };
                    result.Add(tmp);
                }
            }
            return result;
        }
    }
}
