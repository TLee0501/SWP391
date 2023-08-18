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

        public async Task<int> AcceptTeamProjectrequest(Guid teamId)
        {
            var pts = await _context.TeamRequests.Where(a => a.Team == teamId).ToListAsync();
            if (pts == null) return 1;

            var projectTeamId = Guid.NewGuid();
            if (pts[0].ProjectId == null || pts[0].ProjectId == Guid.Empty)
            {
                var projectTeam = new ProjectTeam
                {
                    ProjectTeamId = projectTeamId,
                    TeamName = pts[0].TeamName,
                    TimeStart = DateTime.Now,
                    Status = 1
                };
                await _context.ProjectTeams.AddAsync(projectTeam);
            }
            else
            {
                var projectTeam = new ProjectTeam
                {
                    ProjectTeamId = projectTeamId,
                    ProjectId = (Guid)pts[0].ProjectId,
                    TeamName = pts[0].TeamName,
                    TimeStart = DateTime.Now,
                    Status = 1
                };
                await _context.ProjectTeams.AddAsync(projectTeam);
            }

            foreach (var item in pts)
            {
                if (item.Status.Equals("1")) return 2;
                item.Status = "1";

                var teamMember = new TeamMember
                {
                    TeamMemberId = Guid.NewGuid(),
                    ProjectTeamId = projectTeamId,
                    UserId = item.UserId
                };
                await _context.TeamMembers.AddAsync(teamMember);
            }

            try
            {
                await _context.SaveChangesAsync();
                return 3;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<int> DenyTeamProjectrequest(Guid teamId)
        {
            var pts = await _context.TeamRequests.Where(a => a.Team == teamId).ToListAsync();
            if (pts == null) return 1;
            foreach (var item in pts)
            {
                if (item.Status.Equals("2")) return 2;
                item.Status = "2";
            }
            try
            {
                await _context.SaveChangesAsync();
                return 3;
            } catch (Exception ex)
            {
                return 0;
            }
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
            var listTeamId = new List<Guid>();

            //Lay list team
            var listRequest = await _context.TeamRequests.Where(a => a.ClassId == classId && a.Status == "0").ToListAsync();
            var uniqueListTeam = listRequest.DistinctBy(a => a.Team).ToList();

            //Xu ly tung Team
            foreach (var item in uniqueListTeam)
            {
                var listBasicMember = new List<UserBasicResponse>();
                var listTR = await _context.TeamRequests.Where(a => a.Team == item.Team).ToListAsync();
                var uniqueListTR = listTR.DistinctBy(a =>a.Team).ToList();
                foreach (var item1 in uniqueListTR)
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
                    if (item1.ProjectId != Guid.Empty || item1.ProjectId != null)
                    {
                        var projectTmp = await _context.Projects.FindAsync(item1.ProjectId);
                        var tmpResult = new TeamRequestResponse()
                        {
                            TeamId = item1.Team,
                            ProjectId = item1.ProjectId,
                            ProjectName = projectTmp.ProjectName,
                            Users = listBasicMember
                        };
                        result.Add(tmpResult);
                    }
                    else
                    {
                        var tmpResult = new TeamRequestResponse()
                        {
                            TeamId = item1.Team,
                            Users = listBasicMember
                        };
                        result.Add(tmpResult);
                    }
                }
            }
            return result;
        }

        public async Task<int> StudentCreateTeamRequest(StudentCreateTeamRequest request)
        {
            var checkName = await _context.TeamRequests.Where(a => a.ClassId == request.ClassId && a.TeamName == request.TeamName).ToListAsync();
            if (checkName.Count > 0) return 3;

            var checkclass = await _context.Classes.FindAsync(request.ClassId);
            if (checkclass == null) return 1;

            var team = Guid.NewGuid();
            foreach (var item in request.ListStudent)
            {
                if (request.ProjectId == null)
                {
                    var tmp = new TeamRequest
                    {
                        RequestId = Guid.NewGuid(),
                        UserId = item,
                        ClassId = request.ClassId,
                        Team = team,
                        Status = "0"
                    };
                    await _context.TeamRequests.AddAsync(tmp);
                }
                else
                {
                    var tmp = new TeamRequest
                    {
                        RequestId = Guid.NewGuid(),
                        UserId = item,
                        ClassId = request.ClassId,
                        Team = team,
                        ProjectId = request.ProjectId,
                        Status = "0"
                    };
                    await _context.TeamRequests.AddAsync(tmp);
                }
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