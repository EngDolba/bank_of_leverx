using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankOfLeverx.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
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
        public async Task<ActionResult<Customer>> Get(int customerKey)
        {
            var customer = await _customerService.GetByIdAsync(customerKey);
            if (customer is null)
            {
                return NotFound($"Customer with Key {customerKey} not found.");
            }
            return Ok(customer);
        }

        /// <summary>
        /// Get all customers.
        /// </summary>
        ///
        /// <returns>
        /// A list of all customer objects.
        /// </returns>
        [HttpGet(Name = "GetCustomers")]
        public async Task<IEnumerable<Customer>> Get()
        {
            return await _customerService.GetAllAsync();
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
        public async Task<ActionResult<Customer>> Post([FromBody] CustomerDTO customer)
        {
            var newCustomer = await _customerService.CreateAsync(customer);
            return Ok(newCustomer);
        }

        /// <summary>
        /// Partially update an existing customer.
        /// </summary>
        ///
        /// <param name="customerKey">
        /// The unique key of the customer.
        /// </param>
        ///
        /// <param name="customerPatch">
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
        public async Task<ActionResult> Patch(int customerKey, [FromBody] CustomerPatchDTO customerPatch)
        {
            try
            {
                var updated = await _customerService.PatchAsync(customerKey, customerPatch);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Customer with key: {customerKey} not found");
            }


        }

        /// <summary>
        /// Change an existing customer by providing full object.
        /// </summary>
        ///
        /// <param name="customerKey">
        /// The unique key of the customer.
        /// </param>
        ///
        /// <param name="customer">
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
        [HttpPut("{customerKey}", Name = "PutCustomer")]
        public async Task<ActionResult<Customer>> Put(int customerKey, [FromBody] CustomerDTO customer)
        {
            try
            {
                var updated = await _customerService.UpdateAsync(customerKey, customer);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Customer with key: {customerKey} not found");
            }
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
        public async Task<IActionResult> Delete(int customerKey)
        {
            var deleted = await _customerService.DeleteAsync(customerKey);
            if (!deleted)
            {
                _logger.LogWarning($"Customer with key: {customerKey} not found for deletion.");
                return NotFound($"Customer with key: {customerKey} not found");
            }
            return Ok($"Customer with key: {customerKey} deleted");
        }
    }
}