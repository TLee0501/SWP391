using BusinessObjects.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ProjectService
{
    public interface IProjectService
    {
        Task<ProjectResponse> GetProjectByID(Guid projectID);
        Task<int> CreateProject();
    }
}
