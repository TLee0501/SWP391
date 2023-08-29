using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Azure.Core;

namespace Service.ProjectTeamService
{
    public class ProjectTeamService : IProjectTeamServise
    {
        private readonly Swp391onGoingReportContext _context;

        public ProjectTeamService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<int> AddMember(Guid projectTeamId, Guid userId)
        {
            var pt = await _context.ProjectTeams.FindAsync(projectTeamId);

            var project = await _context.Projects.SingleOrDefaultAsync(a => a.ProjectId == pt.ProjectId);
            var projects = await _context.Projects.Where(a => a.ClassId == project.ClassId && a.IsDeleted == false).ToListAsync();

            //projectTeam
            //var projectTeamId = new List<Guid>();
            var projectTeams = new List<ProjectTeam>();
            foreach (var item in projects)
            {
                var projectTeam = await _context.ProjectTeams.Where(a => a.ProjectId == item.ProjectId && a.Status == 1).ToListAsync();
                foreach (var item1 in projectTeam)
                {
                    //projectTeamId.Add(item1.ProjectTeamId);
                    projectTeams.Add(item1);
                }
            }

            //check user dup
            var users = new List<TeamMember>();
            foreach (var item in projectTeams)
            {
                var tm = await _context.TeamMembers.Where(a => a.ProjectTeamId == item.ProjectTeamId).ToListAsync();
                foreach (var item1 in tm)
                {
                    if (item1.UserId == userId) return 1;
                    users.Add(item1);
                }
            }

            try
            {
                var model = new TeamMember()
                {
                    TeamMemberId = Guid.NewGuid(),
                    ProjectTeamId = projectTeamId,
                    UserId = userId
                };
                await _context.TeamMembers.AddAsync(model);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex) { return 0; }
        }

        public async Task<int> CreateTeam(Guid leaderId, ProjectTeamCreateRequest request)
        {

            var checkProject = await _context.Projects
                .Include(x => x.Class)
                .Where(x => x.ProjectId == request.ProjectId)
                .SingleOrDefaultAsync();

            if (checkProject == null) return 1;

            var @class = checkProject.Class;
            var canCreateTeam = CanCreateTeam(@class);
            if (!canCreateTeam)
            {
                return 20;
            }

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
                .Include(x => x.Project)
                    .ThenInclude(x => x.Class)
                        .ThenInclude(x => x.User)
                .Include(x => x.TeamMembers)
                    .ThenInclude(x => x.User)
                .Include(x => x.Project)
                .Where(x => x.ProjectTeamId == teamId)
                .Where(x => x.TeamMembers.SingleOrDefault(_ => _.UserId == userId) != null);

            var team = await query.SingleOrDefaultAsync();
            if (team == null) return null;

            var @class = team.Project.Class;
            var teacher = team.Project.Class.User;
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
                .Where(x => !x.IsDeleted)
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
                }).ToList(),
                Instructor = new ProjectTeamInstructor
                {
                    Id = teacher.UserId,
                    FullName = teacher.FullName,
                },
                ReportStartTime = @class.ReportStartDate,
                ReportEndTime = @class.ReportEndDate,
            };

        }

        public async Task<ProjectTeamDetailResponse> GetProjectTeamDetailByTeacher(Guid teamId)
        {
            var query = _context.ProjectTeams
                .Include(x => x.Project)
                    .ThenInclude(x => x.Class)
                        .ThenInclude(x => x.User)
                .Include(x => x.TeamMembers)
                    .ThenInclude(x => x.User)
                .Include(x => x.Project)
                .Where(x => x.ProjectTeamId == teamId);

            var team = await query.SingleOrDefaultAsync();
            if (team == null) return null;

            var @class = team.Project.Class;
            var teacher = team.Project.Class.User;
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
                .Where(x => !x.IsDeleted)
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
                }).ToList(),
                Instructor = new ProjectTeamInstructor
                {
                    Id = teacher.UserId,
                    FullName = teacher.FullName,
                },
                ReportStartTime = @class.ReportStartDate,
                ReportEndTime = @class.ReportEndDate,
            };

        }

        public async Task<List<ProjectTeamListResponse>> GetJoinedProjectTeams(Guid userId, Guid classId)
        {
            var query = _context.ProjectTeams
                .Include(x => x.Project)
                    .ThenInclude(x => x.Class)
                        .ThenInclude(x => x.User)
                .Include(x => x.TeamMembers)
                    .ThenInclude(x => x.User)
                .Where(x => x.Project.ClassId == classId)
                .Where(x => x.TeamMembers.SingleOrDefault(_ => _.UserId == userId) != null);
            var teams = await query.ToListAsync();
            List<ProjectTeamListResponse> result = new List<ProjectTeamListResponse>();

            foreach (var team in teams)
            {
                var teacher = team.Project.Class.User;
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
                    },
                    Instructor = new ProjectTeamInstructor
                    {
                        Id = teacher!.UserId,
                        FullName = teacher.FullName,
                    }
                };
                result.Add(data);
            }

            return result;
        }

        public async Task<List<ProjectTeamListResponse>> GetProjectTeamsByTeacher(Guid teacherId)
        {
            var query = _context.ProjectTeams
                .Include(x => x.Project)
                    .ThenInclude(x => x.Class)
                        .ThenInclude(x => x.User)
                .Include(x => x.TeamMembers)
                    .ThenInclude(x => x.User)
                .Where(x => x.Project.Class.User.UserId == teacherId);

            var teams = await query.ToListAsync();
            List<ProjectTeamListResponse> result = new List<ProjectTeamListResponse>();

            foreach (var team in teams)
            {
                var teacher = team.Project.Class.User;
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
                    },
                    Instructor = new ProjectTeamInstructor
                    {
                        Id = teacher!.UserId,
                        FullName = teacher.FullName,
                    }
                };
                result.Add(data);
            }

            return result;
        }

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

        public async Task<int> RemoveMember(Guid projectTeamId, Guid userId)
        {
            var member = await _context.TeamMembers.SingleOrDefaultAsync(a => a.ProjectTeamId == projectTeamId && a.UserId == userId);
            if (member == null) return 1;

            var checkLeader = await _context.ProjectTeams.SingleOrDefaultAsync(a => a.ProjectTeamId == projectTeamId && a.LeaderId == userId && a.Status != 3);
            if (checkLeader != null) return 3;
            try
            {
                _context.TeamMembers.Remove(member);
                await _context.SaveChangesAsync();
                return 2;
            }
            catch (Exception ex) { return 0; }
        }

        private bool CanCreateTeam(Class @class)
        {
            var startTime = @class.RegisterTeamStartDate;
            var endTime = @class.RegisterTeamEndDate;

            if (startTime == null || endTime == null)
            {
                return false;
            }

            var now = DateTime.Now;

            return now >= startTime && now <= endTime;
        }
    }
}