using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace Service.TeamReportService
{
    public class TeamReportService : ITeamReportService
    {
        private readonly Swp391onGoingReportContext _context;
        public TeamReportService(Swp391onGoingReportContext context)
        {
            _context = context;
        }


        public async Task<int> CreateTeamReport(Guid reporterId, CreateTeamReportRequest request)
        {
            var team = await _context.ProjectTeams
                .Include(x => x.Project)
                    .ThenInclude(x => x.Class)
                .Where(x => x.ProjectTeamId == request.TeamId)
                .SingleOrDefaultAsync();
            if (team == null)
            {
                return 1;
            }

            var canSendReport = CanSendReport(team.Project.Class);
            if (!canSendReport)
            {
                return 2;
            }

            var teamReport = new TeamReport
            {
                Id = Guid.NewGuid(),
                TeamId = request.TeamId,
                ReporterId = reporterId,
                Title = request.Title,
                OverviewReport = request.OverviewReport,
                DoneReport = request.DoneReport,
                DoingReport = request.DoingReport,
                TodoReport = request.TodoReport,
                Period = request.Period,
                CreatedDate = DateTime.Now,
            };
            await _context.TeamReports.AddAsync(teamReport);
            await _context.SaveChangesAsync();

            return 0;
        }

        public async Task<bool> CreateTeamReportFeedback(CreateTeamReportFeedback request)
        {
            var existingReport = await _context.TeamReports
                .Include(x => x.TeacherFeedback)
                .SingleOrDefaultAsync();

            if (existingReport == null || existingReport.TeacherFeedback != null) return false;

            var feedback = new TeamReportFeedback
            {
                Id = Guid.NewGuid(),
                Content = request.Content,
                Grade = request.Grade,
                CreatedDate = DateTime.Now,
                TeamReportId = request.ReportId,
            };
            await _context.TeamReportFeedbacks.AddAsync(feedback);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<TeamReportDetailResponse?> GetTeamReport(Guid reportId)
        {
            var teamReport = await _context.TeamReports
                .Include(x => x.TeacherFeedback)
                .Include(x => x.Reporter)
                .Include(x => x.Team)
                    .ThenInclude(x => x.Project)
                        .ThenInclude(x => x.Class)
                            .ThenInclude(x => x.User)
                .Where(x => x.Id == reportId)
                .SingleOrDefaultAsync();

            if (teamReport == null)
            {
                return null;
            }

            var teacher = teamReport.Team.Project.Class.User;

            TeamReportDetailResponseFeedback? feedback = null;
            if (teamReport.TeacherFeedback != null)
            {
                feedback = new TeamReportDetailResponseFeedback
                {
                    Id = teamReport.TeacherFeedback.Id,
                    Content = teamReport.TeacherFeedback.Content!,
                    Grade = teamReport.TeacherFeedback.Grade,
                    CreatedDate = teamReport.TeacherFeedback.CreatedDate,
                    FeedbackBy = new FeedbackBy
                    {
                        Id = teacher!.UserId,
                        FullName = teacher.FullName,
                    }
                };
            }

            var reporter = teamReport.Reporter;

            return new TeamReportDetailResponse
            {
                Id = teamReport.Id,
                Title = teamReport.Title,
                OverviewReport = teamReport.OverviewReport,
                DoneReport = teamReport.DoneReport,
                DoingReport = teamReport.DoingReport,
                TodoReport = teamReport.TodoReport,
                CreatedDate = teamReport.CreatedDate,
                Feedback = feedback,
                Reporter = new TeamReporter
                {
                    Id = reporter.UserId,
                    FullName = reporter.FullName,
                    Code = reporter.Mssv!,
                    Email = reporter.Email,
                },
                Period = teamReport.Period,
            };
        }

        public async Task<List<TeamReportDetailResponse>> GetTeamReports(Guid teamId)
        {
            var teamReports = await _context.TeamReports
                .Include(x => x.Reporter)
                .Include(x => x.TeacherFeedback)
                .Where(x => x.TeamId == teamId)
                .ToListAsync();

            if (teamReports == null)
            {
                return new List<TeamReportDetailResponse>();
            }


            var list = teamReports.Select(teamReport => new TeamReportDetailResponse
            {
                Id = teamReport.Id,
                Title = teamReport.Title,
                OverviewReport = teamReport.OverviewReport,
                DoneReport = teamReport.DoneReport,
                DoingReport = teamReport.DoingReport,
                TodoReport = teamReport.TodoReport,
                CreatedDate = teamReport.CreatedDate,
                Period = teamReport.Period,
                Reporter = new TeamReporter
                {
                    Id = teamReport.Reporter.UserId,
                    FullName = teamReport.Reporter.FullName,
                    Code = teamReport.Reporter.Mssv!,
                    Email = teamReport.Reporter.Email,
                },
                Feedback = teamReport.TeacherFeedback != null ? new TeamReportDetailResponseFeedback
                {
                    Id = teamReport.TeacherFeedback.Id,
                    Content = teamReport.TeacherFeedback.Content!,
                    Grade = teamReport.TeacherFeedback.Grade,
                    CreatedDate = teamReport.TeacherFeedback.CreatedDate,
                } : null,
            }).ToList();
            var sortedList = list.OrderBy(x => x.Period).ToList();
            return sortedList;
        }

        private bool CanSendReport(Class classData)
        {
            if (classData.ReportStartDate == null || classData.ReportEndDate == null)
            {
                return false;
            }

            var now = DateTime.Now;
            return now >= classData.ReportStartDate && now <= classData.ReportEndDate;
        }
    }
}

