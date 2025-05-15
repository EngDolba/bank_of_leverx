using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private  readonly ILogger<CustomersController> _logger;

        private  static int currentKey = 1000;

        private static  List<Customer> Customers = new List<Customer>();    


        public CustomersController(ILogger<CustomersController> logger)
        {
            _logger = logger;

        }

        [HttpGet(Name ="GetCustomers")]
        public IEnumerable<Customer> get()
        {
            return Customers;
        }

        [HttpGet("{customerKey}", Name = "GetCustomer")]
        public ActionResult<Customer> Get(int customerKey)
        {
            var customer = Customers.FirstOrDefault(e => e.Key == customerKey);
            if (customer == null)
            {
                return NotFound($"Customer with Key {customerKey} not found.");
            }
            return Ok(customer);
        }

        [HttpPost(Name = "PostCustomer")]
        public ActionResult<Customer> Post([FromBody] CustomerDTO customer)
        { 
            
            Customer cust = new Customer
            {
                Key = currentKey++,
                Name = customer.Name,
                Surname = customer.Surname,
                Category = customer.Category,
                Branch = customer.Branch
            };
            Customers.Add(cust);
            return Ok(customer);
        }
        [HttpPatch("{customerKey}",Name ="PatchCustomer")] 
        public ActionResult<Customer> patch(int customerKey, [FromBody] CustomerPatchDto customer)
        {
            Customer cust = Customers.FirstOrDefault(e => e.Key == customerKey);
            if (cust == null)
            {
                return NotFound($"Customer with Key {customerKey} not found."); 
            }
            if (customer.Name != null)
            {
                cust.Name = customer.Name;
            }
            if (customer.Surname != null)
            {
                cust.Surname = customer.Surname;
            }
            if (customer.Category != null)
            {
                cust.Category = customer.Category;
            }
            if (customer.Branch != null)
            {
                cust.Branch = customer.Branch;
            }
            return Ok(cust);

        }
        
    }

}
