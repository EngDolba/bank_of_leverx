using bankOfLeverx.Models;
using Microsoft.AspNetCore.Mvc;

namespace bankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        private static readonly string[] Names = ["Nikoloz", "Archil", "Nini"];
        private static readonly string[] Surnames = ["Dolbaia", "Gachechiladze", "Logua"];
        private static readonly string[] Positions = ["PL/SQL Developer", "CEO", "Product Owner"];

        private static int initialEmployeeSize = 3;
        private static int currentEmployeeKey = 1000;
        private static readonly List<Employee> Employees = new List<Employee>();

        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ILogger<EmployeesController> logger)
        {
            for (int index = 0; index < initialEmployeeSize;)
            {
                Employees.Add(new Employee
                {
                    Key = currentEmployeeKey + 1,
                    Name = Names[initialEmployeeSize - 1],
                    Surname = Surnames[initialEmployeeSize - 1],
                    Position = Positions[initialEmployeeSize - 1],
                    Branch = "HDOF" // Head Office
                });
                currentEmployeeKey++;
                initialEmployeeSize--;
            }

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
        public ActionResult<Employee> Get(int employeeKey)
        {
            var employee = Employees.FirstOrDefault(e => e.Key == employeeKey);
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
        public IEnumerable<Employee> get()
        {
            return Employees;
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
        public IActionResult post([FromBody] EmployeeDTO emp)
        {
            Employees.Add(new Employee
            {
                Key = currentEmployeeKey + 1,
                Name = emp.Name,
                Surname = emp.Surname,
                Position = emp.Position,
                Branch = emp.Branch
            });
            currentEmployeeKey++;
            return Ok(Employees.FirstOrDefault(e => e.Key == currentEmployeeKey));
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
        public ActionResult Patch(int employeeKey, [FromBody] EmployeePatchDTO employeePatch)
        {
            var employee = Employees.FirstOrDefault(e => e.Key == employeeKey);
            if (employee is null)
            {
                return NotFound($"Employee with Key {employeeKey} not found.");
            }

            if (employeePatch.Name is not null)
                employee.Name = employeePatch.Name;

            if (employeePatch.Surname is not null)
                employee.Surname = employeePatch.Surname;

            if (employeePatch.Position is not null)
                employee.Position = employeePatch.Position;

            if (employeePatch.Branch is not null)
                employee.Branch = employeePatch.Branch;

            return Ok(employee);
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
        public ActionResult<Employee> Put(int employeeKey, [FromBody] EmployeeDTO employee)
        {
            var emp = Employees.FirstOrDefault(e => e.Key == employeeKey);
            if (emp is null)
            {
                return NotFound($"Employee with Key {employeeKey} not found.");
            }
            emp.Name = employee.Name;
            emp.Surname = employee.Surname;
            emp.Position = employee.Position;
            emp.Branch = employee.Branch;
            return Ok(emp);
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
        public IActionResult delete(int employeeKey)
        {
            Employee cust = Employees.FirstOrDefault(e => e.Key == employeeKey);
            if (cust is null)
            {
                return NotFound($"Employee with key: {employeeKey} not found");
            }
            Employees.Remove(cust);
            return Ok($"Employee with key: {employeeKey} deleted");
        }
    }
}