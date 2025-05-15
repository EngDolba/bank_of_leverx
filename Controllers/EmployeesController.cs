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
        ///<summary>change one employee</summary>
        ///<param name="employeeKey"></param>
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
        ///<param name="employeeKey"></param>
        ///<param name="employeePatch"></param>
        ///<returns>changed customer with all fields</returns> 
        [HttpPatch("{employeeKey}")] 

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



    }
}

