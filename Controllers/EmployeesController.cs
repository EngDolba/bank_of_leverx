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

        private static int currentlEmployeeId = 1000;

        private static readonly List<Employee> Employees = new List<Employee>();


        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(ILogger<EmployeesController> logger)
        {
            for (int index = 0; index < initialEmployeeSize;)
            {

                Employees.Add(new Employee
                {
                    ID = currentlEmployeeId+1,
                    Name = Names[initialEmployeeSize - 1],
                    Surname = Surnames[initialEmployeeSize - 1],
                    Position = Positions[initialEmployeeSize - 1],
                    Branch = "HDOF" // "Head Office"
                });
                currentlEmployeeId++;
                initialEmployeeSize--;
            }

            _logger = logger;
        }

         [HttpGet("{employeeId}", Name = "GetEmployee")]
        public ActionResult<Employee> Get(int employeeId)
        {
            var employee = Employees.FirstOrDefault(e => e.ID == employeeId);
            if (employee == null)
            {
                return NotFound($"Employee with ID {employeeId} not found.");
            }
            return Ok(employee);
        }

        [HttpGet(Name = "GetEmployees")]
        public IEnumerable<Employee> get()
        {
            return Employees;
        }

        [HttpPut(Name = "PutEmployee")]
        public IActionResult put([FromBody] EmployeeDTO emp)
        {
            Employees.Add(new Employee
            {
                ID = currentlEmployeeId + 1,
                Name = emp.Name,
                Surname = emp.Surname,
                Position = emp.Position,
                Branch = emp.Branch
            });
            currentlEmployeeId++;
            return Ok(Employees.FirstOrDefault(e => e.ID == currentlEmployeeId));
        }

        [HttpPatch("{employeeId}")]
        public IActionResult Patch(int employeeId, [FromBody] EmployeePatchDTO employeePatch)
        {
            var employee = Employees.FirstOrDefault(e => e.ID == employeeId);
            if (employee == null)
            {
                return NotFound($"Employee with ID {employeeId} not found.");
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

