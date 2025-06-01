using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionsController> _logger;

        public TransactionsController(ITransactionRepository transactionRepository, ILogger<TransactionsController> logger)
        {
            _transactionRepository = transactionRepository;
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
            return await _transactionRepository.GetAllAsync();
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
            var transaction = await _transactionRepository.GetByIdAsync(TransactionKey);
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
        public async Task<IActionResult> Post([FromBody] TransactionDTO Transaction)
        {
            var newTransaction = await _transactionRepository.CreateAsync(Transaction);
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
            var updated = await _transactionRepository.PatchAsync(TransactionKey, Transaction);
            if (updated is null)
            {
                return NotFound($"Transaction with Key {TransactionKey} not found.");
            }
            return Ok(updated);
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
            var updated = await _transactionRepository.UpdateAsync(TransactionKey, Transaction);
            if (updated is null)
            {
                return NotFound($"Transaction with Key {TransactionKey} not found.");
            }
            return Ok(updated);
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
            var deleted = await _transactionRepository.DeleteAsync(TransactionKey);
            if (!deleted)
            {
                return NotFound($"Transaction with key: {TransactionKey} not found");
            }
            return Ok($"Transaction with key: {TransactionKey} deleted");
        }
    }
}