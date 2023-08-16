using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ProjectTeamService
{
    public class ProjectTeamService : IProjectTeamServise
    {
        private readonly Swp391onGoingReportContext _context;

        public ProjectTeamService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<ProjectTeamResponse> getProjectTeamById(Guid projectTeamId)
        {
            var pt = await _context.ProjectTeams.FindAsync(projectTeamId);
            if (pt == null) return null;
            var project = await _context.Projects.FindAsync(pt.ProjectId);
            var result = new ProjectTeamResponse
            {
                ProjectTeamId = projectTeamId,
                ProjectId = pt.ProjectId,
                ProjectName = project.ProjectName,
                TeamName = pt.TeamName,
                TimeStart = pt.TimeStart,  
                TimeEnd = pt.TimeEnd,
                Status = pt.Status
            };
            return result;
        }

        public async Task<int> StudentCreateTeamRequest(StudentCreateTeamRequest request)
        {
            var checkclass = await _context.Classes.FindAsync(request.ClassID);
            if (checkclass == null) return 1;

            var team = Guid.NewGuid();
            foreach (var item in request.ListStudent)
            {
                var tmp = new TeamRequest
                {
                    RequestId = Guid.NewGuid(),
                    UserId = item,
                    ClassId = request.ClassID,
                    Team = team,
                    Status = "Chờ duyệt"
                };
                await _context.TeamRequests.AddAsync(tmp);
            }
            try
            {
                await _context.SaveChangesAsync();
                return 2;
            } catch (Exception ex)
            {
                return 0;
            }
        }
    }
}