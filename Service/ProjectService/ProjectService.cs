﻿using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Service.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly Swp391onGoingReportContext _context;

        public ProjectService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public async Task<int> CreateProject(ProjectCreateRequest request)
        {
            var check = await _context.Projects.SingleOrDefaultAsync(a => a.ProjectName == request.ProjectName && a.ClassId == request.ClassId && a.IsDeleted == false);
            if (check != null) return 2;

            var project = new Project
            {
                ClassId = request.ClassId,
                ProjectId = Guid.NewGuid(),
                ProjectName = request.ProjectName,
                Description = request.Description,
                FunctionalReq = request.FunctionalReq,
                NonfunctionalReq = request.NonfunctionalReq,
                IsDeleted = false
            };
            try
            {
                await _context.Projects.AddAsync(project);
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<ProjectResponse> GetProjectByID(Guid projectId)
        {
            var projectTeam = await _context.ProjectTeams
                .Include(_ => _.TeamMembers)
                    .ThenInclude(_ => _.User)
                .Include(_ => _.Project)
                    .ThenInclude(_ => _.Class)
                .Where(_ => _.ProjectId == projectId).SingleOrDefaultAsync();

            var result = new ProjectResponse
            {
                ProjectId = projectId,
                ProjectName = projectTeam!.Project.ProjectName,
                Description = projectTeam.Project.Description,
                ClassID = projectTeam.Project.Class.ClassId,
                ClassName = projectTeam.Project.Class.ClassName,
                IsSelected = projectTeam.Project.IsSelected,
                FunctionalReq = projectTeam.Project.FunctionalReq,
                NonfunctionalReq = projectTeam.Project.NonfunctionalReq,
                Members = projectTeam.TeamMembers.Select(_ => new ProjectMemberResponse
                {
                    MemberId = _.User.UserId,
                    MemberFullName = _.User.FullName,
                    MemberCode = _.User.Mssv!,
                }).ToList(),
            };
            return result;
        }

        public async Task<List<ProjectResponse>> GetProjectsByClassID(Guid classId)
        {
            var result = new List<ProjectResponse>();
            var projects = await _context.Projects.Where(a => a.ClassId == classId && a.IsDeleted == false).ToListAsync();
            if (projects == null) return null;

            foreach (var item in projects)
            {
                var classTmp = await _context.Classes.FindAsync(item.ClassId);
                var tmp = new ProjectResponse
                {
                    ProjectId = item.ProjectId,
                    ProjectName = item.ProjectName,
                    ClassID = item.ClassId,
                    ClassName = classTmp.ClassName,
                    Description = item.Description,
                    FunctionalReq = item.FunctionalReq,
                    NonfunctionalReq = item.NonfunctionalReq,
                    IsSelected = item.IsSelected
                };
                result.Add(tmp);
            }
            return result;
        }

        private async Task<List<ProjectResponse>> GetProjectsByClassIDandUserID(Guid classId, Guid userId)
        {
            var result = new List<ProjectResponse>();
            var projects = await _context.Projects.Where(a => a.ClassId == classId && a.IsDeleted == false).ToListAsync();
            if (projects == null) return null;

            foreach (var item in projects)
            {
                var projectTeam = await _context.ProjectTeams.Where(a => a.ProjectId == item.ProjectId && a.Status == 1).ToListAsync();
                if (projectTeam != null)
                {
                    foreach (var item1 in projectTeam)
                    {
                        var teamMember = await _context.TeamMembers.Where(a => a.ProjectTeamId == item1.ProjectTeamId).ToListAsync();
                        if (teamMember != null)
                        {
                            foreach (var item2 in teamMember)
                            {
                                if (item2.UserId == userId)
                                {
                                    var classTmp = await _context.Classes.FindAsync(classId);
                                    var tmp = new ProjectResponse
                                    {
                                        ProjectId = item.ProjectId,
                                        ProjectName = item.ProjectName,
                                        ClassID = item.ClassId,
                                        ClassName = classTmp.ClassName,
                                        Description = item.Description,
                                        FunctionalReq = item.FunctionalReq,
                                        NonfunctionalReq = item.NonfunctionalReq,
                                        IsSelected = item.IsSelected
                                    };
                                    result.Add(tmp);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ProjectResponse>> GetProjectsByFilter(Guid classId, Guid userId, string? searchName, bool hasUserId)
        {
            var result = new List<ProjectResponse>();
            if (hasUserId == true)
                result = await GetProjectsByClassIDandUserID(classId, userId);
            else if (hasUserId == false)
                result = await SearchProjectInClass(classId, searchName);
            return result;
        }

        private async Task<List<ProjectResponse>> SearchProjectInClass(Guid classId, string? searchName)
        {
            var project = await _context.Projects.Where(a => a.ClassId == classId && a.IsDeleted == false).ToListAsync();
            if (project == null) return null;

            var classTmp = await _context.Classes.FindAsync(classId);
            var result = new List<ProjectResponse>();
            if (searchName.IsNullOrEmpty())
            {
                foreach (var item in project)
                {
                    var projectTmp = new ProjectResponse
                    {
                        ProjectId = item.ProjectId,
                        ProjectName = item.ProjectName,
                        ClassID = item.ClassId,
                        ClassName = classTmp.ClassName,
                        Description = item.Description,
                        FunctionalReq = item.FunctionalReq,
                        NonfunctionalReq = item.NonfunctionalReq,
                        IsSelected = item.IsSelected
                    };
                    result.Add(projectTmp);
                }
            }
            else
            {
                foreach (var item in project)
                {
                    if (item.ProjectName.ToLower().Contains(searchName.ToLower()))
                    {
                        var projectTmp = new ProjectResponse
                        {
                            ProjectId = item.ProjectId,
                            ProjectName = item.ProjectName,
                            ClassID = item.ClassId,
                            ClassName = classTmp.ClassName,
                            Description = item.Description,
                            FunctionalReq = item.FunctionalReq,
                            NonfunctionalReq = item.NonfunctionalReq,
                            IsSelected = item.IsSelected
                        };
                        result.Add(projectTmp);
                    }
                }
            }
            return result;
        }

        public async Task<int> UpdateProject(ProjectUpdateRequest request)
        {
            var project = await _context.Projects.FindAsync(request.ProjectId);
            if (project == null) return 1;

            project.ProjectName = request.ProjectName;
            project.Description = request.Description;
            project.FunctionalReq = request.FunctionalReq;
            project.NonfunctionalReq = request.NonfunctionalReq;

            try
            {
                await _context.SaveChangesAsync();
                return 2;
            }
            catch { return 0; }
        }

        /*public async Task<int> DeleteProject(Guid projectId)
        {
            var check = await _context.Projects.FindAsync(projectId);
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
        }*/

        public async Task<List<ProjectResponse>> GetAllProjectsInClass(Guid classId, string? searchName)
        {
            var query = _context.Projects.Include(_ => _.Class).Where(_ => _.ClassId == classId && !_.IsDeleted);
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(_ => _.ProjectName.Contains(searchName));
            }
            var list = await query.ToListAsync();
            var result = list.Select(item => new ProjectResponse
            {
                ProjectId = item.ProjectId,
                ProjectName = item.ProjectName,
                ClassID = item.ClassId,
                ClassName = item.Class.ClassName,
                Description = item.Description,
                FunctionalReq = item.FunctionalReq,
                NonfunctionalReq = item.NonfunctionalReq,
                IsSelected = item.IsSelected
            }).ToList();

            return result;
        }

        public async Task<List<ProjectResponse>> GetWorkingProjectsInClass(Guid userId, Guid classId)
        {
            var query = _context.TeamMembers.Where(_ => _.UserId == userId)
                .Include(_ => _.ProjectTeam)
                    .ThenInclude(x => x.Project)
                    .ThenInclude(x => x.Class)
                .Where(_ => _.ProjectTeam.Project.ClassId == classId);

            var list = await query.ToListAsync();
            var result = list.Select(item => new ProjectResponse
            {
                ProjectId = item.ProjectTeam.Project.ProjectId,
                ProjectName = item.ProjectTeam.Project.ProjectName,
                ClassID = item.ProjectTeam.Project.ClassId,
                ClassName = item.ProjectTeam.Project.Class.ClassName,
                Description = item.ProjectTeam.Project.Description,
                FunctionalReq = item.ProjectTeam.Project.FunctionalReq,
                NonfunctionalReq = item.ProjectTeam.Project.NonfunctionalReq,
                IsSelected = item.ProjectTeam.Project.IsSelected
            }).ToList();

            return result;
        }

        public async Task<List<ProjectAndStatusResponse>> GetProjectsAndStatusByClassIDandUserID(Guid classId, Guid userId)
        {
            var result = new List<ProjectAndStatusResponse>();
            var projects = await _context.Projects.Where(a => a.ClassId == classId).ToListAsync();
            if (projects.IsNullOrEmpty()) return null;

            foreach (var item in projects)
            {
                var classTmp = await _context.Classes.FindAsync(item.ClassId);

                var tmp = new ProjectAndStatusResponse
                {
                    ProjectId = item.ProjectId,
                    ProjectName = item.ProjectName,
                    ClassID = item.ClassId,
                    ClassName = classTmp.ClassName,
                    Description = item.Description,
                    IsSelected = item.IsSelected
                };
                result.Add(tmp);
            }
            return result;
        }
    }
}
