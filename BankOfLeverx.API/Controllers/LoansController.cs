using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanRepository _loanRepository;
        private readonly ILogger<LoansController> _logger;

        public LoansController(ILoanRepository loanRepository, ILogger<LoansController> logger)
        {
            _loanRepository = loanRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get a specific loan by key.
        /// </summary>
        ///
        /// <param name="loanKey">
        /// The unique key of the loan.
        /// </param>
        ///
        /// <returns>
        /// The loan object if found.
        /// </returns>
        ///
        /// <response code="200">
        /// Loan found and returned.
        /// </response>
        /// <response code="404">
        /// Loan not found.
        /// </response>
        [HttpGet("{loanKey}", Name = "GetLoan")]
        public async Task<ActionResult<Loan>> Get(int loanKey)
        {
            var loan = await _loanRepository.GetByIdAsync(loanKey);
            if (loan is null)
            {
                return NotFound($"Loan with Key {loanKey} not found.");
            }
            return Ok(loan);
        }

        /// <summary>
        /// Get all loans.
        /// </summary>
        ///
        /// <returns>
        /// A list of all loan objects.
        /// </returns>
        [HttpGet(Name = "GetLoans")]
        public async Task<IEnumerable<Loan>> Get()
        {
            return await _loanRepository.GetAllAsync();
        }

        /// <summary>
        /// Add a new loan.
        /// </summary>
        ///
        /// <param name="loanDto">
        /// Loan object without the key.
        /// </param>
        ///
        /// <returns>
        /// The added loan with assigned key.
        /// </returns>
        ///
        /// <response code="200">
        /// Loan successfully created.
        /// </response>
        [HttpPost(Name = "PostLoan")]
        public async Task<IActionResult> Post([FromBody] LoanDTO loanDto)
        {
            var newLoan = await _loanRepository.CreateAsync(loanDto);
            return Ok(newLoan);
        }

        /// <summary>
        /// Partially update an existing loan.
        /// </summary>
        ///
        /// <param name="loanKey">
        /// The unique key of the loan.
        /// </param>
        ///
        /// <param name="loanPatch">
        /// Loan patch object.
        /// </param>
        ///
        /// <returns>
        /// The updated loan object.
        /// </returns>
        ///
        /// <response code="200">
        /// Loan successfully updated.
        /// </response>
        /// <response code="404">
        /// Loan not found.
        /// </response>
        [HttpPatch("{loanKey}", Name = "PatchLoan")]
        public async Task<ActionResult> Patch(int loanKey, [FromBody] LoanPatchDTO loanPatch)
        {
            var updated = await _loanRepository.PatchAsync(loanKey, loanPatch);
            if (updated is null)
            {
                return NotFound($"Loan with Key {loanKey} not found.");
            }
            return Ok(updated);
        }

        /// <summary>
        /// Change an existing loan by providing full object.
        /// </summary>
        ///
        /// <param name="loanKey">
        /// The unique key of the loan.
        /// </param>
        ///
        /// <param name="loanDto">
        /// The new loan data (excluding the key).
        /// </param>
        ///
        /// <returns>
        /// The updated loan object.
        /// </returns>
        ///
        /// <response code="200">
        /// Loan successfully replaced.
        /// </response>
        /// <response code="404">
        /// Loan not found.
        /// </response>
        [HttpPut("{loanKey}", Name = "PutLoan")]
        public async Task<ActionResult<Loan>> Put(int loanKey, [FromBody] LoanDTO loanDto)
        {
            var updated = await _loanRepository.UpdateAsync(loanKey, loanDto);
            if (updated is null)
            {
                return NotFound($"Loan with Key {loanKey} not found.");
            }
            return Ok(updated);
        }

        /// <summary>
        /// Delete a loan by key.
        /// </summary>
        ///
        /// <param name="loanKey">
        /// The unique key of the loan to delete.
        /// </param>
        ///
        /// <returns>
        /// Status message about the deletion.
        /// </returns>
        ///
        /// <response code="200">
        /// Loan successfully deleted.
        /// </response>
        /// <response code="404">
        /// Loan not found.
        /// </response>
        [HttpDelete("{loanKey}", Name = "deleteLoan")]
        public async Task<IActionResult> Delete(int loanKey)
        {
            var deleted = await _loanRepository.DeleteAsync(loanKey);
            if (!deleted)
            {
                return NotFound($"Loan with key: {loanKey} not found");
            }
            return Ok($"Loan with key: {loanKey} deleted");
        }
    }
}