using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using BankOfLeverx.Application.Interfaces;


namespace BankOfLeverx.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
        {
            _accountService = accountService;
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
        public async Task<IEnumerable<Account>> Get()
        {
            return await _accountService.GetAllAsync();
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
        public async Task<ActionResult<Account>> Get(int AccountKey)
        {
            var account = await _accountService.GetByIdAsync(AccountKey);
            if (account is null)
            {
                return NotFound($"Account with Key {AccountKey} not found.");
            }
            return Ok(account);
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
        public async Task<ActionResult<Account>> Post([FromBody] AccountDTO Account)
        {
            var newAccount = await _accountService.CreateAsync(Account);
            return Ok(newAccount);
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
        public async Task<ActionResult<Account>> Patch(int AccountKey, [FromBody] AccountPatchDTO Account)
        {
            try
            {
                var updated = await _accountService.PatchAsync(AccountKey, Account);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Account with Key {AccountKey} not found."); 

            }
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
        public async Task<ActionResult<Account>> Put(int AccountKey, [FromBody] AccountDTO Account)
        {
            try
            { 
                var updated = await _accountService.UpdateAsync(AccountKey, Account);
                return Ok(updated);
            }
            catch(KeyNotFoundException)
            {
                return NotFound($"Account with Key {AccountKey} not found.");
            }
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
        public async Task<IActionResult> Delete(int AccountKey)
        {
            var deleted = await _accountService.DeleteAsync(AccountKey);
            if (!deleted)
            {
                return NotFound($"Account with key: {AccountKey} not found");
            }
            return Ok($"Account with key: {AccountKey} deleted");
        }
    }
}
