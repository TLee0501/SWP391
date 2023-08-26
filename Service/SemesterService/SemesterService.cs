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
            var checkname = await _context.Semesters.Where(a => a.SemeterName == request.SemeterName).ToListAsync();
            if (!checkname.IsNullOrEmpty()) return 1;

            var semesters = await _context.Semesters.ToListAsync();
            foreach (var item in semesters)
            {
                if (item.StartTime.Date <= request.StartTime.Date && item.EndTime.Date >= request.StartTime.Date)
                    return 2;
                else if (item.StartTime.Date <= request.EndTime.Date && item.EndTime.Date >= request.EndTime.Date)
                    return 3;
            }

            try
            {
                var semester = new Semester
                {
                    SemesterId = Guid.NewGuid(),
                    SemeterName = request.SemeterName,
                    StartTime = request.StartTime.Date,
                    EndTime = request.EndTime.Date
                };
                await _context.Semesters.AddAsync(semester);
                await _context.SaveChangesAsync();
                return 4;
            } catch (Exception ex) { return 0; }
        }

        public async Task<SemesterResponse> GetSemester(Guid semesterId)
        {
            var result = await _context.Semesters.FindAsync(semesterId);
            if (result == null) return null;
            var model = new SemesterResponse()
            {
                SemesterId = result.SemesterId,
                SemeterName = result.SemeterName,
                StartTime = result.StartTime,
                EndTime = result.EndTime
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
                    SemeterName = item.SemeterName,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime
                };
                list.Add(tmp);
            }
            return list;
        }

        public async Task<int> UpdateSemester(Guid semesterId, SemesterCreateRequest request)
        {
            var semester = await _context.Semesters.FindAsync(semesterId);
            if (semester == null) return 1;

            if(request.SemeterName != semester.SemeterName)
            {
                var checkname = await _context.Semesters.Where(a => a.SemeterName == request.SemeterName).ToListAsync();
                if (!checkname.IsNullOrEmpty()) return 2;
            }

            var semesters = await _context.Semesters.ToListAsync();
            foreach (var item in semesters)
            {
                if (request.StartTime.Date != semester.StartTime.Date)
                    if (item.StartTime.Date <= request.StartTime.Date && item.EndTime.Date >= request.StartTime.Date)
                        return 3;
                if (request.EndTime.Date != semester.EndTime.Date)
                    if (item.StartTime.Date <= request.EndTime.Date && item.EndTime.Date >= request.EndTime.Date)
                        return 4;
            }

            try
            {
                semester.SemeterName = request.SemeterName;
                semester.StartTime = request.StartTime;
                semester.EndTime = request.EndTime;
                await _context.SaveChangesAsync();
                return 5;
            } catch (Exception ex) { return 0; }
        }
    }
}
