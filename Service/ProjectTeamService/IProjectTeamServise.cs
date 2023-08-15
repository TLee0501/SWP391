using BusinessObjects.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ProjectTeamService
{
    public interface IProjectTeamServise
    {
        Task<ProjectTeamResponse> getProjectTeamById(Guid projectTeamId);
    }
}
