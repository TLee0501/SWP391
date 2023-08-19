using BusinessObjects;
using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;


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

        public async Task<ClassResponse?> GetClassByID(Guid classId)
        {
            var query = _context.Classes.Include(item => item.User).Include(item => item.Course).Where(item => item.ClassId == classId);
            var result = await query.SingleOrDefaultAsync();
            if (result == null) return null;


            return new ClassResponse
            {
                ClassId = classId,
                ClassName = result.ClassName,
                UserId = result.UserId,
                EnrollCode = result.EnrollCode,
                StartTime = result.TimeStart,
                EndTime = result.TimeEnd,
                CourseCode = result.Course.CourseCode,
                CourseName = result.Course.CourseName,
                TeacherName = result.User.FullName,
            }; ;
        }

        public async Task<List<ClassResponse>> GetClasses(Guid userID, string? role, Guid? courseId = null, string? searchText = null)
        {

            IQueryable<Class> query = _context.Classes.Include(item => item.Course).Include(item => item.User);

            // Allow students to get all classes
            if (!role!.Equals(Roles.STUDENT))
            {
                query = query.Where(item => item.UserId == userID);
            }

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
                TeacherName = item.User.FullName,
            });

            return classResponses.ToList();
        }
    }
}
