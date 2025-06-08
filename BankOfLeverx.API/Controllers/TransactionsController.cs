using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Application.Services;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankOfLeverx.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(
            ITransactionService transactionService,
            ILogger<TransactionsController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        /// <summary>
        /// Get all transactions.
        /// </summary>
        ///
        /// <returns>
        /// A list of all transaction objects.
        /// </returns>
        [HttpGet(Name = "GetTransactions")]
        public async Task<IEnumerable<Transaction>> Get()
        {
            return await _transactionService.GetAllAsync();
        }

        /// <summary>
        /// Get a specific transaction by key.
        /// </summary>
        ///
        /// <param name="TransactionKey">
        /// The unique key of the transaction.
        /// </param>
        ///
        /// <returns>
        /// The transaction object if found.
        /// </returns>
        ///
        /// <response code="200">
        /// Transaction found and returned.
        /// </response>
        /// <response code="404">
        /// Transaction not found.
        /// </response>
        [HttpGet("{TransactionKey}", Name = "GetTransaction")]
        public async Task<ActionResult<Transaction>> Get(int TransactionKey)
        {
            var transaction = await _transactionService.GetByIdAsync(TransactionKey);
            if (transaction is null)
            {
                return NotFound($"Transaction with Key {TransactionKey} not found.");
            }
            return Ok(transaction);
        }

        /// <summary>
        /// Add a new transaction.
        /// </summary>
        ///
        /// <param name="Transaction">
        /// Transaction object without the key.
        /// </param>
        ///
        /// <returns>
        /// The added transaction with assigned key.
        /// </returns>
        ///
        /// <response code="200">
        /// Transaction successfully created.
        /// </response>
        [HttpPost(Name = "PostTransaction")]
        public async Task<ActionResult<Transaction>> Post([FromBody] TransactionDTO Transaction)
        {
            var newTransaction = await _transactionService.CreateAsync(Transaction);
            return Ok(newTransaction);
        }

        /// <summary>
        /// Partially update an existing transaction.
        /// </summary>
        ///
        /// <param name="TransactionKey">
        /// The unique key of the transaction.
        /// </param>
        ///
        /// <param name="Transaction">
        /// Transaction patch object.
        /// </param>
        ///
        /// <returns>
        /// The updated transaction object.
        /// </returns>
        ///
        /// <response code="200">
        /// Transaction successfully updated.
        /// </response>
        /// <response code="404">
        /// Transaction not found.
        /// </response>
        [HttpPatch("{TransactionKey}", Name = "PatchTransaction")]
        public async Task<ActionResult> Patch(int TransactionKey, [FromBody] TransactionPatchDTO Transaction)
        {
            try
            {
                var updated = await _transactionService.PatchAsync(TransactionKey, Transaction);
                return Ok(updated);
            }
            catch (KeyNotFoundException
            )
            {
               return NotFound($"Transaction with Key {TransactionKey} not found:");
            }
        }

        /// <summary>
        /// Change an existing transaction by providing full object.
        /// </summary>
        ///
        /// <param name="TransactionKey">
        /// The unique key of the transaction.
        /// </param>
        ///
        /// <param name="Transaction">
        /// The new transaction data (excluding the key).
        /// </param>
        ///
        /// <returns>
        /// The updated transaction object.
        /// </returns>
        ///
        /// <response code="200">
        /// Transaction successfully replaced.
        /// </response>
        /// <response code="404">
        /// Transaction not found.
        /// </response>
        [HttpPut("{TransactionKey}", Name = "PutTransaction")]
        public async Task<ActionResult<Transaction>> Put(int TransactionKey, [FromBody] TransactionDTO Transaction)
        {
            try
            {
                var updated = await _transactionService.UpdateAsync(TransactionKey, Transaction);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Transaction with Key {TransactionKey} not found.");
            }
        }
        /// <summary>
        /// Delete a transaction by key.
        /// </summary>
        ///
        /// <param name="TransactionKey">
        /// The unique key of the transaction to delete.
        /// </param>
        ///
        /// <returns>
        /// Status message about the deletion.
        /// </returns>
        ///
        /// <response code="200">
        /// Transaction successfully deleted.
        /// </response>
        /// <response code="404">
        /// Transaction not found.
        /// </response>
        [HttpDelete("{TransactionKey}", Name = "deleteTransaction")]
        public async Task<IActionResult> Delete(int TransactionKey)
        {
            var deleted = await _transactionService.DeleteAsync(TransactionKey);
            if (!deleted)
            {
                return NotFound($"Transaction with key: {TransactionKey} not found");
            }
            return Ok($"Transaction with key: {TransactionKey} deleted");
        }
    }
}