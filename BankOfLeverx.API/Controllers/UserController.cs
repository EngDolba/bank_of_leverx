using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankOfLeverx.Controllers
{
    [Authorize(Roles ="1")]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _UserService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService UserService, ILogger<UsersController> logger)
        {
            _UserService = UserService;
            _logger = logger;
        }

        /// <summary>
        /// Get a specific User by key.
        /// </summary>
        ///
        /// <param name="UserKey">
        /// The unique key of the User.
        /// </param>
        ///
        /// <returns>
        /// The User object if found.
        /// </returns>
        ///
        /// <response code="200">
        /// User found and returned.
        /// </response>
        /// <response code="404">
        /// User not found.
        /// </response>
        [Authorize]
        [HttpGet("{UserKey}", Name = "GetUser")]
        public async Task<ActionResult<User>> Get(int UserKey)
        {
            var User = await _UserService.GetByIdAsync(UserKey);
            if (User is null)
            {
                return NotFound($"User with Key {UserKey} not found.");
            }
            return Ok(User);
        }

        /// <summary>
        /// Get all Users.
        /// </summary>
        ///
        /// <returns>
        /// A list of all User objects.
        /// </returns>
        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<User>> Get()
        {
            return await _UserService.GetAllAsync();
        }

        /// <summary>
        /// Add a new User.
        /// </summary>
        ///
        /// <param name="User">
        /// User object without the key.
        /// </param>
        ///
        /// <returns>
        /// The added User with assigned key.
        /// </returns>
        ///
        /// <response code="200">
        /// User successfully created.
        /// </response>
        [HttpPost(Name = "PostUser")]
        public async Task<ActionResult<User>> Post([FromBody] UserDTO User)
        {
            var newUser = await _UserService.CreateAsync(User);
            return Ok(newUser);
        }

        /// <summary>
        /// Partially update an existing User.
        /// </summary>
        ///
        /// <param name="UserKey">
        /// The unique key of the User.
        /// </param>
        ///
        /// <param name="UserPatch">
        /// User patch object.
        /// </param>
        ///
        /// <returns>
        /// The updated User object.
        /// </returns>
        ///
        /// <response code="200">
        /// User successfully updated.
        /// </response>
        /// <response code="404">
        /// User not found.
        /// </response>
        [HttpPatch("{UserKey}", Name = "PatchUser")]
        public async Task<ActionResult> Patch(int UserKey, [FromBody] UserPatchDTO UserPatch)
        {
            try
            {
                var updated = await _UserService.PatchAsync(UserKey, UserPatch);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with key: {UserKey} not found");
            }
            
            
        }

        /// <summary>
        /// Change an existing User by providing full object.
        /// </summary>
        ///
        /// <param name="UserKey">
        /// The unique key of the User.
        /// </param>
        ///
        /// <param name="User">
        /// The new User data (excluding the key).
        /// </param>
        ///
        /// <returns>
        /// The updated User object.
        /// </returns>
        ///
        /// <response code="200">
        /// User successfully replaced.
        /// </response>
        /// <response code="404">
        /// User not found.
        /// </response>
        [HttpPut("{UserKey}", Name = "PutUser")]
        public async Task<ActionResult<User>> Put(int UserKey, [FromBody] UserDTO User)
        {
            try
            {
                var updated = await _UserService.UpdateAsync(UserKey, User);
                return Ok(updated);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"User with key: {UserKey} not found");
            }
        }


        /// <summary>
        /// Delete a User by key.
        /// </summary>
        ///
        /// <param name="UserKey">
        /// The unique key of the User to delete.
        /// </param>
        ///
        /// <returns>
        /// Status message about the deletion.
        /// </returns>
        ///
        /// <response code="200">
        /// User successfully deleted.
        /// </response>
        /// <response code="404">
        /// User not found.
        /// </response>
        [HttpDelete("{UserKey}", Name = "deleteUser")]
        public async Task<IActionResult> Delete(int UserKey)
        {
            var deleted = await _UserService.DeleteAsync(UserKey);
            if (!deleted)
            {
                return NotFound($"User with key: {UserKey} not found");
            }
            return Ok($"User with key: {UserKey} deleted");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            try
            {
                var token = await _UserService.AuthenticateAsync(model.Username, model.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Invalid credentials");
            }
           
        }

    }

}