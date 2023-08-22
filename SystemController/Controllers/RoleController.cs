using Microsoft.AspNetCore.Mvc;
using BusinessObjects.ResponseModel;
using Service.RoleService;

namespace SystemController.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public ActionResult<List<RoleResponse>> GetAllRoles()
        {
            var result = _roleService.GetAllRoles();
            return Ok(result);
        }
    }
}

