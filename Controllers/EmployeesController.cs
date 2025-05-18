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
                    Key = currentEmployeeKey+1,
                    Name = Names[initialEmployeeSize - 1],
                    Surname = Surnames[initialEmployeeSize - 1],
                    Position = Positions[initialEmployeeSize - 1],
                    Branch = "HDOF" // "Head Office"
                });
                currentEmployeeKey++;
                initialEmployeeSize--;
            }

            _logger = logger;
        }
        ///<summary>GET method to get one employee</summary>
        ///<param name="employeeKey">unique key of employee</param>
        ///<returns>employee with given Key</returns> 
        [HttpGet("{employeeKey}", Name = "GetEmployee")]
        public ActionResult<Employee> Get(int employeeKey)
        {
            var employee = Employees.FirstOrDefault(e => e.Key == employeeKey);
            if (employee == null)
            {
                return NotFound($"Employee with Key {employeeKey} not found.");
            }
            return Ok(employee);
        }

        ///<summary>GET method to get all the employees</summary>
        ///<returns>all the employees</returns> 
        [HttpGet(Name = "GetEmployees")]
        public IEnumerable<Employee> get()
        {
            return Employees;
        }

        ///<summary>POST method to add one employee</summary>
        ///<param name="emp">employee object without key</param>
        ///<returns>added employee with its Key</returns> 
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
        ///<summary>PATCH method to change one employee</summary>
        ///<param name="employeeKey">unique key of employee</param>
        ///<param name="employeePatch">employee object only with fields that should be changed</param>
        ///<returns>changed employee with all fields</returns> 
        [HttpPatch("{employeeKey}",Name = "PatchEmployee")]

        public ActionResult Patch(int employeeKey, [FromBody] EmployeePatchDTO employeePatch)
        {
            var employee = Employees.FirstOrDefault(e => e.Key == employeeKey);
            if (employee == null)
            {
                return NotFound($"Employee with Key {employeeKey} not found.");
            }

            if (employeePatch.Name != null)
                employee.Name = employeePatch.Name;

            if (employeePatch.Surname != null)
                employee.Surname = employeePatch.Surname;

            if (employeePatch.Position != null)
                employee.Position = employeePatch.Position;

            if (employeePatch.Branch != null)
                employee.Branch = employeePatch.Branch;

            return Ok(employee);
        }

        ///<summary>PUT method to change one employee</summary>
        ///<param name="employeeKey">unique key of employee</param>
        ///<param name="employee">employee object without key</param>
        ///<returns>changed EMployee with all fields</returns>
        [HttpPut("{employeeKey}",Name = "PutEmployee")]
        public ActionResult<Employee> Put(int employeeKey, [FromBody] EmployeeDTO employee)
        {
            var emp = Employees.FirstOrDefault(e => e.Key == employeeKey);
            if (emp == null)
            {
                return NotFound($"Employee with Key {employeeKey} not found.");
            }
            emp.Name = employee.Name;
            emp.Surname = employee.Surname;
            emp.Position = employee.Position;
            emp.Branch = employee.Branch;
            return Ok(emp);
        }

        ///<summary>DELETE method to delete one employee</summary>
        ///<param name="employeeKey">unique key of employee</param>
        ///<returns>only status code of action </returns>
        [HttpDelete("{employeeKey} ", Name = "deleteEmployee")]
        public IActionResult delete(int employeeKey)
        {
            Employee cust = Employees.FirstOrDefault(e => e.Key == employeeKey);
            if (cust == null)
            {
                return NotFound($"Employee with key: {employeeKey} not found");
            }
            Employees.Remove(cust);
            return Ok($"Employee with key: {employeeKey} deleted");
        }



    }
}

