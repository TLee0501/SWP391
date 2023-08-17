using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<TeamRequestResponse>> GetTeamProjectRequests(Guid classId)
        {
            var result = new List<TeamRequestResponse>();
            var listRequest = await _context.TeamRequests.Where(a => a.ClassId == classId && a.Status == "0").ToListAsync();
            var listTeamId = new List<Guid>();

            //Lay list team
            foreach (var item in listRequest)
            {
                listTeamId.Add(item.Team);
            }
            var uniqueListTeamId = listTeamId.Distinct().ToList();

            //Xu ly tung Team
            foreach (var item in listTeamId)
            {
                var listBasicMember = new List<UserBasicResponse>();
                var listTR = await _context.TeamRequests.Where(a => a.Team == item).ToListAsync();
                listTR = listTR.Distinct().ToList();
                foreach (var item1 in listTR)
                {
                    var members = await _context.TeamRequests.Where(a => a.Team == item1.Team).ToListAsync();
                    foreach (var item2 in members)
                    {
                        var name = await _context.Users.FindAsync(item2.UserId);
                        var tmp = new UserBasicResponse
                        {
                            UserId = item2.UserId,
                            FullName = name.FullName
                        };
                        listBasicMember.Add(tmp);
                    }
                    var tmpResult = new TeamRequestResponse()
                    {
                        TeamId = item1.Team,
                        Users = listBasicMember
                    };
                    result.Add(tmpResult);
                }
            }
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
                    Status = "0"
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