using System;
using BusinessObjects.Models;
using BusinessObjects.ResponseModel;

namespace Service.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly Swp391onGoingReportContext _context;

        public RoleService(Swp391onGoingReportContext context)
        {
            _context = context;
        }

        public List<RoleResponse> GetAllRoles()
        {
            var roles = _context.Roles.ToList();
            var result = roles.Select(item => new RoleResponse
            {
                Id = item.RoleId,
                Name = item.RoleName
            }).ToList();

            return result;
        }
    }
}

