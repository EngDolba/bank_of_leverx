using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeeService employeeService, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        /// <summary>
        /// Get a specific employee by key.
        /// </summary>
        ///
        /// <param name="employeeKey">
        /// The unique key of the employee.
        /// </param>
        ///
        /// <returns>
        /// The employee object if found.
        /// </returns>
        ///
        /// <response code="200">
        /// Employee found and returned.
        /// </response>
        /// <response code="404">
        /// Employee not found.
        /// </response>
        [HttpGet("{employeeKey}", Name = "GetEmployee")]
        public async Task<ActionResult<Employee>> Get(int employeeKey)
        {
            var employee = await _employeeService.GetByIdAsync(employeeKey);
            if (employee is null)
            {
                return NotFound($"Employee with Key {employeeKey} not found.");
            }
            return Ok(employee);
        }

        /// <summary>
        /// Get all employees.
        /// </summary>
        ///
        /// <returns>
        /// A list of all employee objects.
        /// </returns>
        [HttpGet(Name = "GetEmployees")]
        public async Task<IEnumerable<Employee>> Get()
        {
            return await _employeeService.GetAllAsync();
        }

        /// <summary>
        /// Add a new employee.
        /// </summary>
        ///
        /// <param name="emp">
        /// Employee object without the key.
        /// </param>
        ///
        /// <returns>
        /// The added employee with assigned key.
        /// </returns>
        ///
        /// <response code="200">
        /// Employee successfully created.
        /// </response>
        [HttpPost(Name = "PostEmployee")]
        public async Task<ActionResult<Employee>> Post([FromBody] EmployeeDTO emp)
        {
            var newEmployee = await _employeeService.CreateAsync(emp);
            return Ok(newEmployee);
        }

        /// <summary>
        /// Partially update an existing employee.
        /// </summary>
        ///
        /// <param name="employeeKey">
        /// The unique key of the employee.
        /// </param>
        ///
        /// <param name="employeePatch">
        /// Employee patch object.
        /// </param>
        ///
        /// <returns>
        /// The updated employee object.
        /// </returns>
        ///
        /// <response code="200">
        /// Employee successfully updated.
        /// </response>
        /// <response code="404">
        /// Employee not found.
        /// </response>
        [HttpPatch("{employeeKey}", Name = "PatchEmployee")]
        public async Task<ActionResult> Patch(int employeeKey, [FromBody] EmployeePatchDTO employeePatch)
        {
            var updated = await _employeeService.PatchAsync(employeeKey, employeePatch);
            if (updated is null)
            {
                return NotFound($"Employee with Key {employeeKey} not found.");
            }
            return Ok(updated);
        }

        /// <summary>
        /// Change an existing employee by providing full object.
        /// </summary>
        ///
        /// <param name="employeeKey">
        /// The unique key of the employee.
        /// </param>
        ///
        /// <param name="employee">
        /// The new employee data (excluding the key).
        /// </param>
        ///
        /// <returns>
        /// The updated employee object.
        /// </returns>
        ///
        /// <response code="200">
        /// Employee successfully replaced.
        /// </response>
        /// <response code="404">
        /// Employee not found.
        /// </response>
        [HttpPut("{employeeKey}", Name = "PutEmployee")]
        public async Task<ActionResult<Employee>> Put(int employeeKey, [FromBody] EmployeeDTO employee)
        {
            try
            {
                var updated = await _employeeService.UpdateAsync(employeeKey, employee);
                return Ok(updated);
            }
            catch(KeyNotFoundException)
            {
                return NotFound($"Employee with Key {employeeKey} not found.");
            }
            
           
        }

        /// <summary>
        /// Delete an employee by key.
        /// </summary>
        ///
        /// <param name="employeeKey">
        /// The unique key of the employee to delete.
        /// </param>
        ///
        /// <returns>
        /// Status message about the deletion.
        /// </returns>
        ///
        /// <response code="200">
        /// Employee successfully deleted.
        /// </response>
        /// <response code="404">
        /// Employee not found.
        /// </response>
        [HttpDelete("{employeeKey}", Name = "deleteEmployee")]
        public async Task<IActionResult> Delete(int employeeKey)
        {
            var deleted = await _employeeService.DeleteAsync(employeeKey);
            if (!deleted)
            {
                return NotFound($"Employee with key: {employeeKey} not found");
            }
            return Ok($"Employee with key: {employeeKey} deleted");
        }
    }
}