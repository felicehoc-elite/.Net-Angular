using EmployeeManagement.Core.DTO;
using EmployeeManagement.Core.Exceptions;
using EmployeeManagement.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Gets employee
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>Employee.</returns>
        [HttpGet("edit/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EmployeeEdit))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<EmployeeEdit>> GetEmployeeForEdit([FromRoute] string id)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeForEditAsync(id);

                if (employee == null)
                {
                    throw new NotFoundException();
                }

                return Ok(employee);
            }
            catch
            {
                return NotFound();
            }
            
        }

        /// <summary>
        /// Gets employees with an optional manager ID.
        /// </summary>
        /// <param name="managerId">The optional manager ID.</param>
        /// <returns>A list of employees.</returns>
        [HttpGet("list/{managerId?}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeView>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EmployeeView>>> GetList([FromRoute] string? managerId)
        {
            var employees = await _employeeService.GetEmployeesAsync(managerId);

            if (employees.Any())
            {
                return Ok(employees);
            }

            return NotFound();
        }

        /// <summary>
        /// Gets list of managers.
        /// </summary>
        /// <returns>A list of managers.</returns>
        [HttpGet("managers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeView>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<EmployeeView>>> GetManagers()
        {
            var managers = await _employeeService.GetAllManagersAsync();

            if (managers.Any())
            {
                return Ok(managers);
            }

            return NotFound();
        }

        /// <summary>
        /// Creates new employee
        /// </summary>
        /// <param name="employee">Employee object</param>
        /// <returns>Ststus</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post([FromBody] EmployeeCreate employee)
        {
            try
            {
                await _employeeService.AddEmployeeWithRolesAsync(employee);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates employee
        /// </summary>
        /// <param name="employee">Employee object</param>
        /// <returns>Ststus</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put([FromBody] EmployeeEdit employee)
        {
            try
            {
                await _employeeService.UpdateEmployeeWithRolesAsync(employee);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deletes employee
        /// </summary>
        /// <param name="id">employee id</param>
        /// <returns>status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _employeeService.DeleteAsync(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
