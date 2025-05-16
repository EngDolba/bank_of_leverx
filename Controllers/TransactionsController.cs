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
        ///<summary>GET method to get all the Transactions</summary>
        ///
        ///<returns>all the Transactions</returns> 
        [HttpGet(Name = "GetTransactions")]
        public IEnumerable<Transaction> get()
        {
            return Transactions;
        }

        ///<summary>GET method to get one Transaction with its id</summary>
        ///<param name="TransactionKey"></param>
        ///<returns>the Transaction with the given id</returns> 
        [HttpGet("{TransactionKey}", Name = "GetTransaction")]
        public ActionResult<Transaction> Get(int TransactionKey)
        {
            var Transaction = Transactions.FirstOrDefault(e => e.Key == TransactionKey);
            if (Transaction == null)
            {
                return NotFound($"Transaction with Key {TransactionKey} not found.");
            }
            return Ok(Transaction);
        }
        ///<summary>POST method to add one Transaction</summary>
        ///<param name="Transaction"></param>
        ///<returns>added Transaction including its id</returns> 
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

        ///<summary>PATCH method to change one Transaction</summary>
        ///<param name="TransactionKey"></param>
        ///<param name="Transaction"></param>
        ///<returns>changed Transaction with all fields</returns> 
        [HttpPatch("{TransactionKey}", Name = "PatchTransaction")]
        public ActionResult<Transaction> patch(int TransactionKey, [FromBody] TransactionPatchDTO Transaction)
        {
            Transaction tr = Transactions.FirstOrDefault(e => e.Key == TransactionKey);
            if (tr == null)
            {
                return NotFound($"Transaction with Key {TransactionKey} not found.");
            }
            if (Transaction.AccountKey != null)
            {
                tr.AccountKey = (int)Transaction.AccountKey;
            }
            if (Transaction.IsDebit != null)
            {
                tr.IsDebit = (bool)Transaction.IsDebit;
            }
            if (Transaction.Amount != null)
            {
                tr.Amount = (double) Transaction.Amount;
            }
            if (Transaction.Date != null)
            {
                tr.Date = (DateTime) Transaction.Date;
            }

            if (Transaction.Category != null)
            {
                tr.Category = Transaction.Category;
            }
            return Ok(tr);

        }

        ///<summary>PUT method to change one Transaction</summary>
        ///<param name="TransactionKey"></param>
        ///<param name="Transaction"></param>
        ///<returns>changed Transaction with all fields</returns>
        [HttpPut("{TransactionKey}", Name = "PutTransaction")]
        public ActionResult<Transaction> put(int TransactionKey, [FromBody] TransactionDTO Transaction)
        {
            Transaction tr = Transactions.FirstOrDefault(e => e.Key == TransactionKey);
            if (tr == null)
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

        ///<summary>DELETE method to delete one Transaction</summary>
        ///<param name="TransactionKey"></param>
        ///<returns>only status code</returns>
        [HttpDelete("{TransactionKey}", Name = "deleteTransaction")]
        public IActionResult delete(int TransactionKey)
        {
            Transaction tr = Transactions.FirstOrDefault(e => e.Key == TransactionKey);
            if (tr == null) {
                return NotFound($"Transaction with key: {TransactionKey} not found");
            }
            Transactions.Remove(tr);
            return Ok($"Transaction with key: {TransactionKey} deleted");
        }
        
    }

}
