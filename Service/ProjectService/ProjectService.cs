using BusinessObjects.Models;
using BusinessObjects.ResponseModel;
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

        /*public async Task<ProjectResponse> GetProjectByID(Guid projectID)
        {
            var project = await _context.Projects.FindAsync(projectID);
            if (project == null) return null;
            var result = new ProjectResponse
            {

            }
        }*/
    }
}
