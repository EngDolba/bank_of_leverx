using bankOfLeverx.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoansController : Controller
    {
        private readonly ILogger<LoansController> _logger;

        private static int currentKey = 1000;

        private static List<Loan> Loans = new List<Loan>();


        public LoansController(ILogger<LoansController> logger)
        {
            _logger = logger;

        }
        ///<summary>GET method to get all the Loans</summary>
        ///
        ///<returns>all the Loans</returns> 
        [HttpGet(Name = "GetLoans")]
        public IEnumerable<Loan> get()
        {
            return Loans;
        }

        ///<summary>GET method to get one Loan with its id</summary>
        ///<param name="LoanKey">unique identifier of loan</param>
        ///<returns>the Loan with the given id</returns> 
        [HttpGet("{LoanKey}", Name = "GetLoan")]
        public ActionResult<Loan> Get(int LoanKey)
        {
            var Loan = Loans.FirstOrDefault(e => e.Key == LoanKey);
            if (Loan == null)
            {
                return NotFound($"Loan with Key {LoanKey} not found.");
            }
            return Ok(Loan);
        }
        ///<summary>POST method to add one Loan</summary>
        ///<param name="Loan">loan object without key</param>
        ///<returns>added Loan including its id</returns> 
        [HttpPost(Name = "PostLoan")]
        public ActionResult<Loan> Post([FromBody] LoanDTO Loan)
        {

            Loan ln = new Loan
            {
                Key = currentKey++,
                Amount = Loan.Amount,
                startDate = Loan.startDate,
                endDate = Loan.endDate,
                Type = Loan.Type,
                AccountKey = Loan.AccountKey
            };

            Loans.Add(ln);
            return Ok(ln);
        }

        ///<summary>PATCH method to change one Loan</summary>
        ///<param name="LoanKey">unique identifier of loan</param>
        ///<param name="Loan">loan object without key</param>
        ///<returns>changed Loan with all fields</returns> 
        [HttpPatch("{LoanKey}", Name = "PatchLoan")]
        public ActionResult<Loan> patch(int LoanKey, [FromBody] LoanPatchDTO Loan)
        {
            Loan ln = Loans.FirstOrDefault(e => e.Key == LoanKey);
            if (ln == null)
            {
                return NotFound($"Loan with Key {LoanKey} not found.");
            }
            if (Loan.AccountKey != null)
            {
                ln.AccountKey = (int)Loan.AccountKey;
            }
            if (Loan.Amount != null)
            {
                ln.Amount = (int)Loan.Amount;
            }
            if (Loan.startDate != null)
            {
                ln.startDate = (DateOnly) Loan.startDate;
            }
            if (Loan.endDate != null)
            {
                ln.endDate = (DateOnly) Loan.endDate;
            }

            if (Loan.Type != null)
            {
                ln.Type = Loan.Type;
            }
            return Ok(ln);

        }

        ///<summary>PUT method to change one Loan</summary>
        ///<param name="LoanKey">unique identifier of loan</param>
        ///<param name="Loan">loan object without key</param>
        ///<returns>changed Loan with all fields</returns>
        [HttpPut("{LoanKey}", Name = "PutLoan")]
        public ActionResult<Loan> put(int LoanKey, [FromBody] LoanDTO Loan)
        {
            Loan ln = Loans.FirstOrDefault(e => e.Key == LoanKey);
            if (ln == null)
            {
                return NotFound($"Loan with Key {LoanKey} not found.");
            }
            ln.Amount = Loan.Amount;
            ln.startDate = Loan.startDate;
            ln.endDate = Loan.endDate;
            ln.Type = Loan.Type;
            ln.AccountKey = Loan.AccountKey;
            return Ok(ln);
        }

        ///<summary>DELETE method to delete one Loan</summary>
        ///<param name="LoanKey">unique identifier of loan</param>
        ///<returns>only status code</returns>
        [HttpDelete("{LoanKey}", Name = "deleteLoan")]
        public IActionResult delete(int LoanKey)
        {
            Loan tr = Loans.FirstOrDefault(e => e.Key == LoanKey);
            if (tr == null) {
                return NotFound($"Loan with key: {LoanKey} not found");
            }
            Loans.Remove(tr);
            return Ok($"Loan with key: {LoanKey} deleted");
        }
        
    }

}
