using BankOfLeverx.Application.CQRS.Commands;
using BankOfLeverx.Application.CQRS.Queries;
using BankOfLeverx.Application.Exceptions;
using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Application.Validators;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankOfLeverx.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly IMediator _loanMediator;
        private readonly LoanValidator _loanValidator;
        private readonly ILoanPaymentService _loanPaymentService;


        public LoansController(IMediator loanService, LoanValidator loanValidator,ILoanPaymentService loanPaymentService)
        {
            _loanMediator = loanService;
            _loanValidator = loanValidator;
            _loanPaymentService = loanPaymentService;
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
            var loan = await _loanMediator.Send(new GetLoanByIdQuery(loanKey));
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
            return await _loanMediator.Send(new GetAllLoansQuery());
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
        public async Task<ActionResult<Loan>> Post([FromBody] LoanDTO loanDto)
        {
            var validationResult = _loanValidator.Validate(loanDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var newLoan = await _loanMediator.Send(new CreateLoanCommand(loanDto));
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

            try
            {
                var updated = await _loanMediator.Send(new PatchLoanCommand(loanKey, loanPatch));
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"loan with Key {loanKey} not found.");

            }
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
            var validationResult = _loanValidator.Validate(loanDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            try
            {
                var updated = await _loanMediator.Send(new UpdateLoanCommand(loanKey, loanDto));
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Loan with Key {loanKey} not found.");
            }
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
            var deleted = await _loanMediator.Send(new DeleteLoanCommand(loanKey));
            if (!deleted)
            {
                return NotFound($"Loan with key: {loanKey} not found");
            }
            return Ok($"Loan with key: {loanKey} deleted");
        }
        /// <summary>
        /// subtract interest from a loan by key.
        /// </summary>
        ///
        /// <param name="loanKey">
        /// The unique key of the loan to delete.
        /// </param>
        ///
        /// <returns>
        /// Status message about the interest payoff.
        /// </returns>
        ///
        /// <response code="200">
        /// interest successfully paid off.
        /// </response>
        /// <response code="404">
        /// Loan not found.
        /// </response>
        /// <response code="422">
        /// request does not make sense on business level
        /// </response>
        [HttpPost("{loanKey}", Name = "interestSubtract")]
        public async Task<ActionResult> subtractInterest(int loanKey)
        {
            try
            {
                var subtracted = await _loanPaymentService.SubtractInterestAsync(loanKey);
                return Ok($"Interest sucessfully paid off from: {loanKey}");

            }
            catch (InsufficientFundsException)
            {
                return UnprocessableEntity($"insufficient balance on account tied with loan {loanKey}");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound($"Loan with Key {loanKey} not found.");
            }
            catch (LoanPaidOffException)
            {
                return UnprocessableEntity($"Loan with Key {loanKey} is already paid off.");
            }
            

        }
    }
}