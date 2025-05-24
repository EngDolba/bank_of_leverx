using bankOfLeverx.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : Controller
    {
        private readonly ILogger<AccountsController> _logger;
        private static int currentKey = 1000;
        private static List<Account> Accounts = new List<Account>();

        public AccountsController(ILogger<AccountsController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get all accounts.
        /// </summary>
        ///
        /// <returns>
        /// A list of all account objects.
        /// </returns>
        [HttpGet(Name = "GetAccounts")]
        public IEnumerable<Account> get()
        {
            return Accounts;
        }

        /// <summary>
        /// Get a specific account by key.
        /// </summary>
        ///
        /// <param name="AccountKey">
        /// The unique key of the account.
        /// </param>
        ///
        /// <returns>
        /// The account object if found.
        /// </returns>
        ///
        /// <response code="200">
        /// Account found and returned.
        /// </response>
        /// <response code="404">
        /// Account not found.
        /// </response>
        [HttpGet("{AccountKey}", Name = "GetAccount")]
        public ActionResult<Account> Get(int AccountKey)
        {
            var Account = Accounts.FirstOrDefault(e => e.Key == AccountKey);
            if (Account == null)
            {
                return NotFound($"Account with Key {AccountKey} not found.");
            }
            return Ok(Account);
        }

        /// <summary>
        /// Add a new account.
        /// </summary>
        ///
        /// <param name="Account">
        /// Account object without the key.
        /// </param>
        ///
        /// <returns>
        /// The added account with assigned key.
        /// </returns>
        ///
        /// <response code="200">
        /// Account successfully created.
        /// </response>
        [HttpPost(Name = "PostAccount")]
        public ActionResult<Account> Post([FromBody] AccountDTO Account)
        {
            Account acc = new Account
            {
                Key = currentKey++,
                Number = Account.Number,
                PlanCode = Account.PlanCode,
                Balance = Account.Balance,
                CustomerKey = Account.CustomerKey,
                Branch = Account.Branch
            };
            Accounts.Add(acc);
            return Ok(acc);
        }

        /// <summary>
        /// Partially update an existing account.
        /// </summary>
        ///
        /// <param name="AccountKey">
        /// The unique key of the account.
        /// </param>
        ///
        /// <param name="Account">
        /// Account patch object.
        /// </param>
        ///
        /// <returns>
        /// The updated account object.
        /// </returns>
        ///
        /// <response code="200">
        /// Account successfully updated.
        /// </response>
        /// <response code="404">
        /// Account not found.
        /// </response>
        [HttpPatch("{AccountKey}", Name = "PatchAccount")]
        public ActionResult<Account> patch(int AccountKey, [FromBody] AccountPatchDTO Account)
        {
            Account acc = Accounts.FirstOrDefault(e => e.Key == AccountKey);
            if (acc == null)
            {
                return NotFound($"Account with Key {AccountKey} not found.");
            }
            if (Account.CustomerKey != null)
            {
                acc.CustomerKey = (int)Account.CustomerKey;
            }
            if (Account.Number != null)
            {
                acc.Number = Account.Number;
            }
            if (Account.PlanCode != null)
            {
                acc.PlanCode = Account.PlanCode;
            }
            if (Account.Balance != null)
            {
                acc.Balance = (double)Account.Balance;
            }
            if (Account.Branch != null)
            {
                acc.Branch = Account.Branch;
            }
            return Ok(acc);
        }

        /// <summary>
        /// Change an existing account by providing full object.
        /// </summary>
        ///
        /// <param name="AccountKey">
        /// The unique key of the account.
        /// </param>
        ///
        /// <param name="Account">
        /// The new account data (excluding the key).
        /// </param>
        ///
        /// <returns>
        /// The updated account object.
        /// </returns>
        ///
        /// <response code="200">
        /// Account successfully replaced.
        /// </response>
        /// <response code="404">
        /// Account not found.
        /// </response>
        [HttpPut("{AccountKey}", Name = "PutAccount")]
        public ActionResult<Account> put(int AccountKey, [FromBody] AccountDTO Account)
        {
            Account acc = Accounts.FirstOrDefault(e => e.Key == AccountKey);
            if (acc == null)
            {
                return NotFound($"Account with Key {AccountKey} not found.");
            }
            acc.Number = Account.Number;
            acc.PlanCode = Account.PlanCode;
            acc.Balance = Account.Balance;
            acc.CustomerKey = Account.CustomerKey;
            acc.Branch = Account.Branch;
            return Ok(acc);
        }

        /// <summary>
        /// Delete an account by key.
        /// </summary>
        ///
        /// <param name="AccountKey">
        /// The unique key of the account to delete.
        /// </param>
        ///
        /// <returns>
        /// Status message about the deletion.
        /// </returns>
        ///
        /// <response code="200">
        /// Account successfully deleted.
        /// </response>
        /// <response code="404">
        /// Account not found.
        /// </response>
        [HttpDelete("{AccountKey}", Name = "deleteAccount")]
        public IActionResult delete(int AccountKey)
        {
            Account acc = Accounts.FirstOrDefault(e => e.Key == AccountKey);
            if (acc == null)
            {
                return NotFound($"Account with key: {AccountKey} not found");
            }
            Accounts.Remove(acc);
            return Ok($"Account with key: {AccountKey} deleted");
        }
    }
}