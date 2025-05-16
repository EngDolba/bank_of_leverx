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
        ///<summary>GET method to get all the Accounts</summary>
        ///
        ///<returns>all the Accounts</returns> 
        [HttpGet(Name = "GetAccounts")]
        public IEnumerable<Account> get()
        {
            return Accounts;
        }

        ///<summary>GET method to get one Account with its id</summary>
        ///<param name="AccountKey"></param>
        ///<returns>the Account with the given id</returns> 
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
        ///<summary>POST method to add one Account</summary>
        ///<param name="Account"></param>
        ///<returns>added Account including its id</returns> 
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

        ///<summary>PATCH method to change one Account</summary>
        ///<param name="AccountKey"></param>
        ///<param name="Account"></param>
        ///<returns>changed Account with all fields</returns> 
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
                acc.CustomerKey = (int) Account.CustomerKey;
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
                acc.Balance = (double) Account.Balance;
            }
            if (Account.Branch != null)
            {
                acc.Branch = Account.Branch;
            }
            return Ok(acc);

        }

        ///<summary>PUT method to change one Account</summary>
        ///<param name="AccountKey"></param>
        ///<param name="Account"></param>
        ///<returns>changed Account with all fields</returns>
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
        ///<summary>DELETE method to delete one Account</summary>
        ///<param name="AccountKey"></param>
        ///<returns>only status code</returns>
        [HttpDelete("{AccountKey}", Name = "deleteAccount")]
        public IActionResult delete(int AccountKey)
        {
            Account acc = Accounts.FirstOrDefault(e => e.Key == AccountKey);
            if (acc == null) {
                return NotFound($"Account with key: {AccountKey} not found");
            }
            Accounts.Remove(acc);
            return Ok($"Account with key: {AccountKey} deleted");
        }
        
    }

}
