using System;
using BusinessObjects.ResponseModel;

namespace Service.RoleService
{
    public interface IRoleService
    {
        public List<RoleResponse> GetAllRoles();
    }
}

