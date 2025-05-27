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

        /// <summary>
        /// Get all loans.
        /// </summary>
        ///
        /// <returns>
        /// A list of all loan objects.
        /// </returns>
        [HttpGet(Name = "GetLoans")]
        public IEnumerable<Loan> get()
        {
            return Loans;
        }

        /// <summary>
        /// Get a specific loan by key.
        /// </summary>
        ///
        /// <param name="LoanKey">
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
        [HttpGet("{LoanKey}", Name = "GetLoan")]
        public ActionResult<Loan> Get(int LoanKey)
        {
            var Loan = Loans.FirstOrDefault(e => e.Key == LoanKey);
            if (Loan is null)
            {
                return NotFound($"Loan with Key {LoanKey} not found.");
            }
            return Ok(Loan);
        }

        /// <summary>
        /// Add a new loan.
        /// </summary>
        ///
        /// <param name="Loan">
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

        /// <summary>
        /// Partially update an existing loan.
        /// </summary>
        ///
        /// <param name="LoanKey">
        /// The unique key of the loan.
        /// </param>
        ///
        /// <param name="Loan">
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
        [HttpPatch("{LoanKey}", Name = "PatchLoan")]
        public ActionResult<Loan> patch(int LoanKey, [FromBody] LoanPatchDTO Loan)
        {
            Loan ln = Loans.FirstOrDefault(e => e.Key == LoanKey);
            if (ln is null)
            {
                return NotFound($"Loan with Key {LoanKey} not found.");
            }
            if (Loan.AccountKey is not null)
            {
                ln.AccountKey = (int)Loan.AccountKey;
            }
            if (Loan.Amount is not null)
            {
                ln.Amount = (int)Loan.Amount;
            }
            if (Loan.startDate is not null)
            {
                ln.startDate = (DateOnly)Loan.startDate;
            }
            if (Loan.endDate is not null)
            {
                ln.endDate = (DateOnly)Loan.endDate;
            }
            if (Loan.Type is not null)
            {
                ln.Type = Loan.Type;
            }
            return Ok(ln);
        }

        /// <summary>
        /// Change an existing loan by providing full object.
        /// </summary>
        ///
        /// <param name="LoanKey">
        /// The unique key of the loan.
        /// </param>
        ///
        /// <param name="Loan">
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
        [HttpPut("{LoanKey}", Name = "PutLoan")]
        public ActionResult<Loan> put(int LoanKey, [FromBody] LoanDTO Loan)
        {
            Loan ln = Loans.FirstOrDefault(e => e.Key == LoanKey);
            if (ln is null)
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

        /// <summary>
        /// Delete a loan by key.
        /// </summary>
        ///
        /// <param name="LoanKey">
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
        [HttpDelete("{LoanKey}", Name = "deleteLoan")]
        public IActionResult delete(int LoanKey)
        {
            Loan tr = Loans.FirstOrDefault(e => e.Key == LoanKey);
            if (tr is null)
            {
                return NotFound($"Loan with key: {LoanKey} not found");
            }
            Loans.Remove(tr);
            return Ok($"Loan with key: {LoanKey} deleted");
        }
    }
}