using bankOfLeverx.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly ILogger<TransactionsController> _logger;
        private static int currentKey = 1000;
        private static List<Transaction> Transactions = new List<Transaction>();

        public TransactionsController(ILogger<TransactionsController> logger)
        {
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
        public IEnumerable<Transaction> get()
        {
            return Transactions;
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
        public ActionResult<Transaction> Get(int TransactionKey)
        {
            var Transaction = Transactions.FirstOrDefault(e => e.Key == TransactionKey);
            if (Transaction is null)
            {
                return NotFound($"Transaction with Key {TransactionKey} not found.");
            }
            return Ok(Transaction);
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
        public ActionResult<Transaction> Post([FromBody] TransactionDTO Transaction)
        {
            Transaction acc = new Transaction
            {
                Key = currentKey++,
                AccountKey = Transaction.AccountKey,
                IsDebit = Transaction.IsDebit,
                Category = Transaction.Category,
                Amount = Transaction.Amount,
                Date = Transaction.Date,
                Comment = Transaction.Comment,
            };

            Transactions.Add(acc);
            return Ok(acc);
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
        public ActionResult<Transaction> patch(int TransactionKey, [FromBody] TransactionPatchDTO Transaction)
        {
            Transaction tr = Transactions.FirstOrDefault(e => e.Key == TransactionKey);
            if (tr is null)
            {
                return NotFound($"Transaction with Key {TransactionKey} not found.");
            }
            if (Transaction.AccountKey is not null)
            {
                tr.AccountKey = (int)Transaction.AccountKey;
            }
            if (Transaction.IsDebit is not null)
            {
                tr.IsDebit = (bool)Transaction.IsDebit;
            }
            if (Transaction.Amount is not null)
            {
                tr.Amount = (double)Transaction.Amount;
            }
            if (Transaction.Date is not null)
            {
                tr.Date = (DateTime)Transaction.Date;
            }
            if (Transaction.Category is not null)
            {
                tr.Category = Transaction.Category;
            }
            return Ok(tr);
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
        public ActionResult<Transaction> put(int TransactionKey, [FromBody] TransactionDTO Transaction)
        {
            Transaction tr = Transactions.FirstOrDefault(e => e.Key == TransactionKey);
            if (tr is null)
            {
                return NotFound($"Transaction with Key {TransactionKey} not found.");
            }
            tr.IsDebit = Transaction.IsDebit;
            tr.Amount = Transaction.Amount;
            tr.Date = Transaction.Date;
            tr.AccountKey = Transaction.AccountKey;
            tr.Comment = Transaction.Comment;
            return Ok(tr);
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
        public IActionResult delete(int TransactionKey)
        {
            Transaction tr = Transactions.FirstOrDefault(e => e.Key == TransactionKey);
            if (tr is null)
            {
                return NotFound($"Transaction with key: {TransactionKey} not found");
            }
            Transactions.Remove(tr);
            return Ok($"Transaction with key: {TransactionKey} deleted");
        }
    }
}