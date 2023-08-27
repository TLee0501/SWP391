using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using BusinessObjects.Enums;
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

        public async Task<int> CreateTeam(Guid leaderId, ProjectTeamCreateRequest request)
        {
            var checkProject = await _context.Projects.FindAsync(request.ProjectId);
            if (checkProject == null) return 1;

            var checkdup = request.Users.Distinct().ToList();
            if (checkdup.Count != request.Users.Count) return 2;
            foreach (var item in request.Users)
            {
                if (leaderId == item) return 2;
            }

            var classId = checkProject.ClassId;
            var projects = await _context.Projects.Where(a => a.ClassId == classId && a.IsDeleted == false).ToListAsync();

            //projectTeam
            var projectTeamId = new List<Guid>();
            var projectTeams = new List<ProjectTeam>();
            foreach (var item in projects)
            {
                var projectTeam = await _context.ProjectTeams.Where(a => a.ProjectId == item.ProjectId && a.Status == 1).ToListAsync();
                foreach (var item1 in projectTeam)
                {
                    projectTeamId.Add(item1.ProjectTeamId);
                    projectTeams.Add(item1);
                }
            }

            //userId in db
            var userIds = new List<Guid>();
            foreach (var item in projectTeamId)
            {
                var tm = await _context.TeamMembers.Where(a => a.ProjectTeamId == item).ToListAsync();
                foreach (var item1 in tm)
                {
                    userIds.Add(item1.UserId);
                }
            }

            //compare 2 list
            var memberInRequest = request.Users;
            memberInRequest.Add(leaderId);
            var duplicateList = userIds.Intersect(memberInRequest).ToList();
            if (!duplicateList.IsNullOrEmpty()) return 3;

            //Tên nhóm
            string newName;
            if (projectTeams.IsNullOrEmpty())
            {
                newName = "G01";
            }
            else
            {
                var nameMax = projectTeams.MaxBy(a => a.TeamName).TeamName;
                string digits = new string(nameMax.Where(char.IsDigit).ToArray());
                string letters = new string(nameMax.Where(char.IsLetter).ToArray());

                int number;
                if (!int.TryParse(digits, out number)) //int.Parse would do the job since only digits are selected
                {
                    Console.WriteLine("Something weired happened");
                }

                newName = letters + (++number).ToString("D2");
            }

            try
            {
                var ptId = Guid.NewGuid();
                var projectTeamModel = new ProjectTeam()
                {
                    ProjectTeamId = ptId,
                    ProjectId = request.ProjectId,
                    TeamName = newName,
                    Status = 1,
                    LeaderId = leaderId
                };
                await _context.ProjectTeams.AddAsync(projectTeamModel);

                foreach (var item in memberInRequest)
                {
                    var tmp = new TeamMember()
                    {
                        TeamMemberId = Guid.NewGuid(),
                        ProjectTeamId = ptId,
                        UserId = item
                    };
                    await _context.TeamMembers.AddAsync(tmp);
                }

                await _context.SaveChangesAsync();
                return 4;
            }
            catch (Exception ex) { return 0; }
        }

        public async Task<ProjectTeamDetailResponse?> GetJoinedProjectTeamById(Guid userId, Guid teamId)
        {
            var query = _context.ProjectTeams
                .Include(x => x.TeamMembers)
                    .ThenInclude(x => x.User)
                .Include(x => x.Project)
                .Where(x => x.ProjectTeamId == teamId)
                .Where(x => x.TeamMembers.SingleOrDefault(_ => _.UserId == userId) != null);

            var team = await query.SingleOrDefaultAsync();
            if (team == null) return null;

            var leader = await _context.Users.FindAsync(team.LeaderId);
            var members = team.TeamMembers.Select(x => new ProjectTeamMember
            {
                Id = x.UserId,
                FullName = x.User.FullName,
                Code = x.User.Mssv!,
                Email = x.User.Email
            }).ToList();
            var project = new ProjectInfo
            {
                Id = team.Project.ProjectId,
                Name = team.Project.ProjectName,
                Description = team.Project.Description,
                FunctionalReq = team.Project.FunctionalReq,
                NonfunctionalReq = team.Project.NonfunctionalReq
            };

            var tasks = await _context.Tasks
                .Include(x => x.StudentTasks)
                    .ThenInclude(x => x.User)
                .Where(x => x.ProjectId == project.Id)
                .ToListAsync();

            return new ProjectTeamDetailResponse
            {
                Id = team!.ProjectTeamId,
                Members = members,
                Project = project,
                Leader = new ProjectTeamMember
                {
                    Id = leader!.UserId,
                    FullName = leader!.FullName,
                    Code = leader!.Mssv!,
                    Email = leader!.Email
                },
                Tasks = tasks.Select(x => new ProjectTask
                {
                    Id = x.TaskId,
                    Name = x.TaskName,
                    Description = x.Description,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    Status = x.Status,
                    Members = x.StudentTasks.Select(item => new ProjectTeamMember
                    {
                        Id = item.User.UserId,
                        FullName = item.User.FullName,
                        Code = item.User.Mssv!,
                        Email = item.User.Email
                    }).ToList(),

                }).ToList()
            };

        }

        public async Task<List<ProjectTeamListResponse>> GetJoinedProjectTeams(Guid userId, Guid classId)
        {
            var query = _context.ProjectTeams
                .Include(x => x.Project)
                .Include(x => x.TeamMembers)
                    .ThenInclude(x => x.User)
                .Where(x => x.Project.ClassId == classId)
                .Where(x => x.TeamMembers.SingleOrDefault(_ => _.UserId == userId) != null);
            var teams = await query.ToListAsync();
            List<ProjectTeamListResponse> result = new List<ProjectTeamListResponse>();

            foreach (var team in teams)
            {
                var leader = await _context.Users.FindAsync(team!.LeaderId);
                var data = new ProjectTeamListResponse
                {
                    Id = team!.ProjectTeamId,
                    Members = team.TeamMembers.Select(x => new ProjectTeamMember
                    {
                        Id = x.UserId,
                        FullName = x.User.FullName,
                        Code = x.User.Mssv!,
                        Email = x.User.Email
                    }).ToList(),
                    Project = new ProjectInfo
                    {
                        Id = team.Project.ProjectId,
                        Name = team.Project.ProjectName,
                        Description = team.Project.Description,
                        FunctionalReq = team.Project.FunctionalReq,
                        NonfunctionalReq = team.Project.NonfunctionalReq
                    },
                    Leader = new ProjectTeamMember
                    {
                        Id = leader!.UserId,
                        FullName = leader!.FullName,
                        Code = leader!.Mssv!,
                        Email = leader!.Email
                    }
                };
                result.Add(data);
            }

            return result;
        }

        /*public async Task<int> AcceptTeamProjectrequest(Guid teamId)
        {
            //check student 
            var checkStudent = await CheckStudentValid(teamId);
            if (checkStudent == false) return 4;

            var teamRequestList = await _context.TeamRequests.Where(_ => _.Team == teamId).ToListAsync();
            if (teamRequestList.IsNullOrEmpty())
            {
                return 1;
            }

            var fistRequest = teamRequestList[0];
            if (fistRequest.Status == TeamRequestStatus.Approved)
            {
                return 1;
            }

            //check if project already choosen
            var checkChoosen = await _context.ProjectTeams.Where(a => a.ProjectId == teamRequestList.First().ProjectId && a.Status != 3).ToListAsync();
            if (!checkChoosen.IsNullOrEmpty()) return 5;

            var projects = await _context.Projects.Where(a => a.ClassId == teamRequestList.First().ClassId && a.IsDeleted == false).ToListAsync();
            var pt = new List<ProjectTeam>();
            foreach (var item in projects)
            {
                var ptTmp = await _context.ProjectTeams.Where(a => a.ProjectId == item.ProjectId).ToListAsync();
                if (!ptTmp.IsNullOrEmpty())
                {
                    foreach (var item1 in ptTmp)
                    {
                        pt.Add(item1);
                    }
                }
            }

            //Tên nhóm
            string newName;
            if (pt.IsNullOrEmpty())
            {
                newName = "G01";
            }
            else
            {
                var nameMax = pt.MaxBy(a => a.TeamName).TeamName;
                string digits = new string(nameMax.Where(char.IsDigit).ToArray());
                string letters = new string(nameMax.Where(char.IsLetter).ToArray());

                int number;
                if (!int.TryParse(digits, out number)) //int.Parse would do the job since only digits are selected
                {
                    Console.WriteLine("Something weired happened");
                }

                newName = letters + (++number).ToString("D2");
            }

            try
            {
                var projectTeamId = Guid.NewGuid();

                var projectTeam = new ProjectTeam
                {
                    ProjectTeamId = projectTeamId,
                    ProjectId = fistRequest.ProjectId,
                    TeamName = newName,
                    TimeStart = DateTime.Now,
                    Status = 1
                };

                await _context.ProjectTeams.AddAsync(projectTeam);

                foreach (var request in teamRequestList)
                {
                    request.Status = TeamRequestStatus.Approved;
                    var teamMember = new TeamMember
                    {
                        TeamMemberId = Guid.NewGuid(),
                        ProjectTeamId = projectTeamId,
                        UserId = request.UserId
                    };
                    await _context.TeamMembers.AddAsync(teamMember);
                }

                await _context.SaveChangesAsync();
                return 3;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }*/

        /*public async Task<int> DeleteProjectTeam(Guid projectTeamId)
        {
            var pt = await _context.ProjectTeams.FindAsync(projectTeamId);
            if (pt == null) return 0;

            pt.Status = 3;

            var tasks = await _context.Tasks.Where(a => a.ProjectId == pt.ProjectId).ToListAsync();
            foreach (var item in tasks)
            {
                item.IsDeleted = true;
            }

            var project = await _context.Projects.FindAsync(pt.ProjectId);
            project.IsSelected = false;
            try
            {
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex) { return 1; }
        }*/

        /*public async Task<int> DenyTeamProjectrequest(Guid teamId)
        {
            var pts = await _context.TeamRequests.Where(a => a.Team == teamId).ToListAsync();
            if (pts == null) return 1;
            foreach (var item in pts)
            {
                if (item.Status.Equals(TeamRequestStatus.Denied)) return 2;
                item.Status = TeamRequestStatus.Denied;
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
        }*/

        public async Task<ProjectTeamResponse> getProjectTeamById(Guid projectTeamId)
        {
            var pt = await _context.ProjectTeams.FindAsync(projectTeamId);
            if (pt == null) return null;
            var project = await _context.Projects.FindAsync(pt.ProjectId);
            var status = "Đang làm";
            if (pt.Status.Equals("2")) status = "Đã hoàn thành";
            else if (pt.Status.Equals("3")) status = "Đã dừng";

            var members = await _context.TeamMembers.Where(a => a.ProjectTeamId == pt.ProjectTeamId).ToListAsync();

            var team = new List<TeamMemberResponse>();
            foreach (var m in members)
            {
                if (m.UserId == pt.LeaderId)
                {
                    var userTmp = await _context.Users.FindAsync(m.UserId);
                    var tmp = new TeamMemberResponse
                    {
                        UserId = userTmp.UserId,
                        FullName = userTmp.FullName,
                        IsLeader = true,
                        Mssv = userTmp.Mssv
                    };
                    team.Add(tmp);
                }
                else
                {
                    var userTmp = await _context.Users.FindAsync(m.UserId);
                    var tmp = new TeamMemberResponse
                    {
                        UserId = userTmp.UserId,
                        FullName = userTmp.FullName,
                        IsLeader = false,
                        Mssv = userTmp.Mssv
                    };
                    team.Add(tmp);
                }
            }

            var result = new ProjectTeamResponse
            {
                ProjectTeamId = projectTeamId,
                ProjectId = (Guid)pt.ProjectId,
                ProjectName = project.ProjectName,
                TeamName = pt.TeamName,
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

                    var team = new List<TeamMemberResponse>();
                    foreach (var m in members)
                    {
                        if (m.UserId == pt.LeaderId)
                        {
                            var userTmp = await _context.Users.FindAsync(m.UserId);
                            var tmp = new TeamMemberResponse
                            {
                                UserId = userTmp.UserId,
                                FullName = userTmp.FullName,
                                IsLeader = true,
                                Mssv = userTmp.Mssv
                            };
                            team.Add(tmp);
                        }
                        else
                        {
                            var userTmp = await _context.Users.FindAsync(m.UserId);
                            var tmp = new TeamMemberResponse
                            {
                                UserId = userTmp.UserId,
                                FullName = userTmp.FullName,
                                IsLeader = false,
                                Mssv = userTmp.Mssv
                            };
                            team.Add(tmp);
                        }
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
                        Users = team,
                        Status = status
                    };
                    result.Add(ptTmp);
                }
            }
            return result;
        }

        /*public async Task<List<TeamRequestResponse>> GetTeamProjectRequests(Guid? userId, Guid classId)
        {
            var result = new List<TeamRequestResponse>();
            var listTeamId = new List<Guid>();

            //Lay list team
            var query = _context.TeamRequests.Where(a => a.ClassId == classId && a.Status != TeamRequestStatus.Cancelled);
            if (userId != null)
            {
                query = query.Where(_ => _.UserId == userId);
            }
            var listRequest = await query.ToListAsync();

            var uniqueListTeam = listRequest.DistinctBy(a => a.Team).ToList();

            //Xu ly tung Team
            foreach (var item in uniqueListTeam)
            {
                var listBasicMember = new List<UserBasicResponse>();
                var listTR = await _context.TeamRequests.Where(a => a.Team == item.Team).ToListAsync();
                var uniqueListTR = listTR.DistinctBy(a => a.Team).ToList();
                foreach (var item1 in uniqueListTR)
                {
                    var members = await _context.TeamRequests.Where(a => a.Team == item1.Team).ToListAsync();
                    foreach (var item2 in members)
                    {
                        var name = await _context.Users.FindAsync(item2.UserId);
                        var tmp = new UserBasicResponse
                        {
                            UserId = item2.UserId,
                            FullName = name.FullName,
                            Mssv = name.Mssv
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
                            Users = listBasicMember,
                            CreatedBy = item1.UserId,
                            Status = item1.Status
                        };
                        result.Add(tmpResult);
                    }
                    else
                    {
                        var tmpResult = new TeamRequestResponse()
                        {
                            TeamId = item1.Team,
                            Users = listBasicMember,
                            CreatedBy = item1.UserId,
                            Status = item1.Status
                        };
                        result.Add(tmpResult);
                    }
                }
            }
            return result;
        }*/

        /*public async Task<int> StudentCreateTeamRequest(StudentCreateTeamRequest request)
        {
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
                    //TeamName = "",
                    ProjectId = request.ProjectId,
                    Status = TeamRequestStatus.Pending
                };
                await _context.TeamRequests.AddAsync(tmp);
            }
            try
            {
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }*/

        /*private async Task<bool> CheckStudentValid(Guid teamId)
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
            var teamRequests = await _context.TeamRequests.Where(a => a.Team == teamId && a.Status == TeamRequestStatus.Pending).ToListAsync();
            foreach (var item in teamRequests)
            {
                userIdsFromRequest.Add(item.UserId);
            }

            //compare 2 list
            var duplicateList = userIds.Intersect(userIdsFromRequest).ToList();
            if (duplicateList.IsNullOrEmpty()) return true;
            return false;
        }*/
    }
}