using System.ComponentModel.DataAnnotations;
using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Service.SemesterService
{
    public class SemesterService : ISemesterService
    {
        private readonly Swp391onGoingReportContext _context;

        public SemesterService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<int> CreateSemester(SemesterCreateRequest request)
        {
            try
            {
                var startTime = Utils.ConvertUTCToLocalDateTime(request.StartTime);
                var endTime = Utils.ConvertUTCToLocalDateTime(request.EndTime);
                var semester = new Semester
                {
                    SemesterId = Guid.NewGuid(),
                    SemeterName = request.SemesterName,
                    StartTime = (DateTime)startTime!,
                    EndTime = (DateTime)endTime!,
                };

                var isValid = await IsValidSemester(semester);
                if (!isValid)
                {
                    return 1;
                }

                await _context.Semesters.AddAsync(semester);
                await _context.SaveChangesAsync();
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<SemesterDetailResponse?> GetSemester(Guid semesterId)
        {
            var result = await _context.Semesters.FindAsync(semesterId);
            if (result == null) return null;

            var classes = await _context.Classes
                .Include(x => x.User)
                .Include(x => x.Course)
                .Where(x => x.SemesterId == semesterId)
                .ToListAsync();

            var model = new SemesterDetailResponse
            {
                Id = result.SemesterId,
                Name = result.SemeterName,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Classes = classes.Select(x => new ClassSemesterDetailResponse
                {
                    Id = x.ClassId,
                    Name = x.ClassName,
                    TeacherId = x.UserId,
                    TeacherName = x.User?.FullName,
                    CourseId = x.CourseId,
                    CourseName = x.Course.CourseName,
                }).ToList()
            };
            return model;
        }

        public async Task<List<SemesterResponse>> GetSemesterList()
        {
            var list = new List<SemesterResponse>();
            var inDB = await _context.Semesters.ToListAsync();
            foreach (var item in inDB)
            {
                var tmp = new SemesterResponse()
                {
                    SemesterId = item.SemesterId,
                    SemesterName = item.SemeterName,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime
                };
                list.Add(tmp);
            }
            var sortedList = list
                .OrderBy(x => x.StartTime)
                    .ThenBy(x => x.EndTime)
                .ToList();
            return sortedList;
        }

        public async Task<int> UpdateSemester(Guid semesterId, SemesterCreateRequest request)
        {
            try
            {
                var semester = await _context.Semesters.FindAsync(semesterId);
                if (semester == null) return 1;

                var startTime = Utils.ConvertUTCToLocalDateTime(request.StartTime);
                var endTime = Utils.ConvertUTCToLocalDateTime(request.EndTime);

                semester.SemeterName = request.SemesterName;
                semester.StartTime = (DateTime)startTime!;
                semester.EndTime = (DateTime)endTime!;

                var isValid = await IsValidSemester(semester);

                if (!isValid)
                {
                    return 1;
                }

                await _context.SaveChangesAsync();
                return 0;
            }
            catch (Exception ex) { return -1; }
        }

        private async Task<bool> IsValidSemester(Semester semester)
        {
            var existingSemesters = await _context.Semesters
                .Where(x => x.SemesterId != semester.SemesterId)
                .ToListAsync();

            bool isValid = existingSemesters.All(x =>
                semester.StartTime > x.EndTime ||
                semester.EndTime < x.StartTime
            );

            return isValid;
        }
    }
}
