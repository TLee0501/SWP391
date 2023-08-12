using BusinessObjects.Models;
using BusinessObjects.RequestModel;
using BusinessObjects.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var project = new Project
            {
                ClassId = request.ClassId,
                ProjectId = Guid.NewGuid(),
                ProjectName = request.ProjectName,
                Description = request.Description,
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

        public async Task<ProjectResponse> GetProjectByID(Guid projectID)
        {
            var project = await _context.Projects.FindAsync(projectID);
            var classTmp = await _context.Classes.FindAsync(project.ClassId);
            if (project == null) return null;
            var result = new ProjectResponse
            {
                ProjectId = projectID,
                ProjectName = project.ProjectName,
                ClassID = project.ClassId,
                ClassName = classTmp.ClassName
            };
            return result;
        }

        public async Task<List<ProjectResponse>> GetProjectsByClassID(Guid classID)
        {
            var result = new List<ProjectResponse>();
            var projects = await _context.Projects.Where(a => a.ClassId == classID && a.IsDeleted == false).ToListAsync();
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
                    Description = item.Description
                };
                result.Add(tmp);
            }
            return result;
        }

        public async Task<List<ProjectResponse>> GetProjectsByClassIDandUserID(Guid classID, Guid userID)
        {
            var result = new List<ProjectResponse>();
            var projects = await _context.Projects.Where(a => a.ClassId == classID && a.IsDeleted == false).ToListAsync();
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
                                if (item2.UserId == userID)
                                {
                                    var classTmp = await _context.Classes.FindAsync(classID);
                                    var tmp = new ProjectResponse
                                    {
                                        ProjectId = item.ProjectId,
                                        ProjectName = item.ProjectName,
                                        ClassID = item.ClassId,
                                        ClassName = classTmp.ClassName,
                                        Description = item.Description
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

        public async Task<List<ProjectResponse>> SearchProjectInClass(Guid classID, string searchName)
        {
            var project = await _context.Projects.Where(a => a.ClassId == classID && a.IsDeleted == false).ToListAsync();
            if (project == null) return null;

            var classTmp = await _context.Classes.FindAsync(classID);
            var result = new List<ProjectResponse>();
            foreach (var item in project)
            {
                if (item.ProjectName.Contains(searchName))
                {
                    var projectTmp = new ProjectResponse
                    {
                        ProjectId = item.ProjectId,
                        ProjectName = item.ProjectName,
                        ClassID = item.ClassId,
                        ClassName = classTmp.ClassName,
                        Description = item.Description
                    };
                    result.Add(projectTmp);
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

            try
            {
                await _context.SaveChangesAsync();
                return 2;
            }
            catch { return 0; }
        }
    }
}
