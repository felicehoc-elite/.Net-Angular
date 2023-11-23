using EmployeeManagement.Core.DTO;
using EmployeeManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;


namespace EmployeeManagement.Api.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService) 
        { 
            _roleService = roleService;
        }

        /// <summary>
        /// Gets all available roles
        /// </summary>
        /// <returns>A list of roles</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RoleView>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RoleView>>> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();

            if (roles.Any())
            {
                return Ok(roles);
            }

            return NotFound();
        }

        /// <summary>
        /// Get specified role names
        /// </summary>
        /// <param name="roleIds">Role Ids for specific roles</param>
        /// <returns>List of role names for requested Ids</returns>
        [HttpPost("names")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<string>>> GetRoleNames([FromBody] int[] roleIds)
        {
            try
            {
                var roleNames = await _roleService.GetRoleNamesAsync(roleIds);

                if (roleNames.Any())
                {
                    return Ok(roleNames);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
