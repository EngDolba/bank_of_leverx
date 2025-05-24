using bankOfLeverx.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;

        private static int currentKey = 1000;

        private static List<Customer> Customers = new List<Customer>();

        public CustomersController(ILogger<CustomersController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all customers.
        /// </summary>
        /// <returns>
        /// A list of all customer objects.
        /// </returns>
        [HttpGet(Name = "GetCustomers")]
        public IEnumerable<Customer> get()
        {
            return Customers;
        }

        /// <summary>
        /// Get a specific customer by key.
        /// </summary>
        ///
        /// <param name="customerKey">
        /// The unique key of the customer.
        /// </param>
        ///
        /// <returns>
        /// The customer object if found.
        /// </returns>
        ///
        /// <response code="200">
        /// Customer found and returned.
        /// </response>
        /// <response code="404">
        /// Customer not found.
        /// </response>
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

        /// <summary>
        /// Add a new customer.
        /// </summary>
        ///
        /// <param name="customer">
        /// Customer object without the key.
        /// </param>
        ///
        /// <returns>
        /// The added customer with assigned key.
        /// </returns>
        ///
        /// <response code="200">
        /// Customer successfully created.
        /// </response>
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
            return Ok(cust);
        }

        /// <summary>
        /// Change an existing customer by providing full object.
        /// </summary>
        ///
        /// <param name="CustomerKey">
        /// The unique key of the customer.
        /// </param>
        ///
        /// <param name="Customer">
        /// The new customer data (excluding the key).
        /// </param>
        ///
        /// <returns>
        /// The updated customer object.
        /// </returns>
        ///
        /// <response code="200">
        /// Customer successfully replaced.
        /// </response>
        /// <response code="404">
        /// Customer not found.
        /// </response>
        [HttpPut("{CustomerKey}", Name = "PutCustomer")]
        public ActionResult<Customer> put(int CustomerKey, [FromBody] CustomerDTO Customer)
        {
            Customer cust = Customers.FirstOrDefault(e => e.Key == CustomerKey);
            if (cust == null)
            {
                return NotFound($"Customer with Key {CustomerKey} not found.");
            }
            cust.Name = Customer.Name;
            cust.Surname = Customer.Surname;
            cust.Category = Customer.Category;
            cust.Branch = Customer.Branch;
            return Ok(cust);
        }

        /// <summary>
        /// Partially update an existing customer.
        /// </summary>
        ///
        /// <param name="customerKey">
        /// The unique key of the customer.
        /// </param>
        ///
        /// <param name="customer">
        /// Customer patch object.
        /// </param>
        ///
        /// <returns>
        /// The updated customer object.
        /// </returns>
        ///
        /// <response code="200">
        /// Customer successfully updated.
        /// </response>
        /// <response code="404">
        /// Customer not found.
        /// </response>
        [HttpPatch("{customerKey}", Name = "PatchCustomer")]
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

        /// <summary>
        /// Delete a customer by key.
        /// </summary>
        ///
        /// <param name="customerKey">
        /// The unique key of the customer to delete.
        /// </param>
        ///
        /// <returns>
        /// Status message about the deletion.
        /// </returns>
        ///
        /// <response code="200">
        /// Customer successfully deleted.
        /// </response>
        /// <response code="404">
        /// Customer not found.
        /// </response>
        [HttpDelete("{customerKey}", Name = "deleteCustomer")]
        public IActionResult delete(int customerKey)
        {
            Customer cust = Customers.FirstOrDefault(e => e.Key == customerKey);
            if (cust == null)
            {
                return NotFound($"Customer with key: {customerKey} not found");
            }
            Customers.Remove(cust);
            return Ok($"Customer with key: {customerKey} deleted");
        }
    }
}