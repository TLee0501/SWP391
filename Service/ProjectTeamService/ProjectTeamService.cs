using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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
            //check student 
            var checkStudent = await CheckStudentValid(teamId);
            if (checkStudent == false) return 4;

            var pts = await _context.TeamRequests.Where(a => a.Team == teamId).ToListAsync();
            if (pts == null) return 1;

            var projectTeamId = Guid.NewGuid();
            var projectTeam = new ProjectTeam
            {
                ProjectTeamId = projectTeamId,
                ProjectId = (Guid)pts[0].ProjectId,
                TeamName = pts[0].TeamName,
                TimeStart = DateTime.Now,
                Status = 1
            };
            await _context.ProjectTeams.AddAsync(projectTeam);
            var project = await _context.Projects.FindAsync(pts[0].ProjectId);
            project.IsSelected = true;

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
                //throw new NotImplementedException(ex.Message);
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
            var status = "Đang làm";
            if (pt.Status.Equals("2")) status = "Đã hoàn thành";
            else if (pt.Status.Equals("3")) status = "Đã dừng";

            var members = await _context.TeamMembers.Where(a => a.ProjectTeamId == pt.ProjectId).ToListAsync();

            var team = new List<UserBasicResponse>();
            foreach (var m in members)
            {
                var userTmp = await _context.Users.FindAsync(m.UserId);
                var tmp = new UserBasicResponse
                {
                    UserId = userTmp.UserId,
                    FullName = userTmp.FullName
                };
                team.Add(tmp);
            }

            var result = new ProjectTeamResponse
            {
                ProjectTeamId = projectTeamId,
                ProjectId = (Guid)pt.ProjectId,
                ProjectName = project.ProjectName,
                TeamName = pt.TeamName,
                TimeStart = pt.TimeStart,  
                TimeEnd = pt.TimeEnd,
                Users = team,
                Status = status
            };
            return result;
        }

        public async Task<List<ProjectTeamResponse>> getProjectTeamInClass(Guid classId)
        {
            var result = new List<ProjectTeamResponse>();
            var projects = await _context.Projects.Where(a => a.ClassId == classId && a.IsDeleted == false).ToListAsync();
            foreach (var p in projects)
            {
                var projectTeams = await _context.ProjectTeams.Where(a => a.ProjectId == p.ProjectId).ToListAsync();

                foreach (var pt in projectTeams)
                {
                    var members = await _context.TeamMembers.Where(a => a.ProjectTeamId == pt.ProjectTeamId).ToListAsync();

                    var team = new List<UserBasicResponse>();
                    foreach (var m in members)
                    {
                        var userTmp = await _context.Users.FindAsync(m.UserId);
                        var tmp = new UserBasicResponse
                        {
                            UserId = userTmp.UserId,
                            FullName = userTmp.FullName
                        };
                        team.Add(tmp);
                    }
                    var project = await _context.Projects.FindAsync(pt.ProjectId);
                    var status = "Đang làm";
                    if (pt.Status.Equals("2")) status = "Đã hoàn thành";
                    else if (pt.Status.Equals("3")) status = "Đã dừng";

                    var ptTmp = new ProjectTeamResponse
                    {
                        ProjectTeamId = pt.ProjectTeamId,
                        ProjectId = pt.ProjectId,
                        ProjectName = project.ProjectName,
                        TeamName = pt.TeamName,
                        TimeStart = pt.TimeStart,
                        TimeEnd = pt.TimeEnd,
                        Users = team,
                        Status = status
                    };
                    result.Add(ptTmp);
                }
            }
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
                    if (item1.ProjectId != null)
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
                var tmp = new TeamRequest
                {
                    RequestId = Guid.NewGuid(),
                    UserId = item,
                    ClassId = request.ClassId,
                    Team = team,
                    TeamName = request.TeamName,
                    ProjectId = request.ProjectId,
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

        private async Task<bool> CheckStudentValid(Guid teamId)
        {
            var classId = _context.TeamRequests.FirstOrDefaultAsync(a => a.Team == teamId).Result.ClassId;
            var projects = await _context.Projects.Where(a => a.ClassId == classId && a.IsDeleted == false).ToListAsync();

            //projectTeam
            var projectTeamId = new List<Guid>();
            foreach (var item in projects)
            {
                var projectTeam = await _context.ProjectTeams.Where(a => a.ProjectId == item.ProjectId && a.Status == 1).ToListAsync();
                foreach (var item1 in projectTeam)
                {
                    projectTeamId.Add(item1.ProjectTeamId);
                }
            }

            //userId
            var userIds = new List<Guid>();
            foreach (var item in projectTeamId)
            {
                var tm = await _context.TeamMembers.Where(a => a.ProjectTeamId == item).ToListAsync();
                foreach (var item1 in tm)
                {
                    userIds.Add(item1.UserId);
                }
            }

            //UserId from request
            var userIdsFromRequest = new List<Guid>();
            var teamRequests = await _context.TeamRequests.Where(a => a.Team == teamId && a.Status == "0").ToListAsync();
            foreach (var item in teamRequests)
            {
                userIdsFromRequest.Add(item.UserId);
            }

            //compare 2 list
            var duplicateList = userIds.Intersect(userIdsFromRequest).ToList();
            if (duplicateList.IsNullOrEmpty()) return true;
            return false;
        }
    }
}