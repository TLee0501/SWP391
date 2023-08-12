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
                    ClassName = classTmp.ClassName
                };
                result.Add(tmp);
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
