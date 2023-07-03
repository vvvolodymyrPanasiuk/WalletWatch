using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Threading.Tasks;
using System;
using WalletWatch.Domain.Entities.UserAggregate;
using WalletWatch.WebAPI.Models.AuthenticationModels.Request;
using System.Linq;
using WalletWatch.WebAPI.Models.AuthenticationModels.Response;
using Microsoft.Extensions.Logging;

namespace WalletWatch.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            ILogger<AuthenticationController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }


        // POST: api/v{version:apiVersion}/<AuthenticationController>/register
        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="model">User data to be registered</param>
        /// <returns>Returns status 201 (Created) if the user was registered successfully or an error message.</returns>
        /// <response code="201">Returns status 201 (Created) if the user was registered successfully.</response>
        /// <response code="400">If the model state is not valid or there was an authentication exception.</response>
        /// <response code="500">If an error occurred during the operation.</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            // Check if user data is valid
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if user with same email already exists
                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        throw new AuthenticationException("A user with this email already exists.");
                    }
                    
                    // Create user with provided data
                    var user = new User
                    {
                        UserName = model.FirstName + model.LastName,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                    };

                    // Save user to database
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        var errorMessage = string.Join("; ", result.Errors.Select(e => e.Description));
                        throw new Exception(errorMessage);
                    }

                    // If user was created successfully, return status 201
                    return StatusCode(StatusCodes.Status201Created);
                }
                catch (AuthenticationException ex)
                {
                    // Handle authentication exception and return 400 response
                    _logger.LogWarning($"Authorization error: {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
                }
                catch (Exception ex)
                {
                    // Log error and return 500 response
                    _logger.LogCritical($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

        //TODO: responce must jwt token for auth

        // POST: api/v{version:apiVersion}/<AuthenticationController>/login
        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="model">User credentials required for login</param>   
        /// <returns>Returns status 200 (OK) with access and refresh tokens if authentication was successful or an error message.</returns>
        /// <response code="200">Returns status 200 (OK) with access and refresh tokens if authentication was successful.</response>
        /// <response code="400">If the model state is not valid or the user metadata is invalid.</response>
        /// <response code="500">If an error occurred during the operation or the tokens are invalid.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Find user by email
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    // If user not found, return authorization error
                    if (user == null)
                    {
                        throw new AuthenticationException("Invalid login or password");
                    }

                    // Check password
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (!result.Succeeded)
                    {
                        throw new AuthenticationException("Invalid username or password");
                    }

                    return StatusCode(StatusCodes.Status200OK, 
                        new LoginResponse()
                        {
                            UserId = user.Id,
                            Email = user.Email,
                            UserName = user.UserName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            SubscriptionStatus = user.SubscriptionStatus
                        });
                }
                catch (AuthenticationException ex)
                {
                    // Handle authentication exception and return 400 response
                    _logger.LogWarning($"Authorization: {ex.Message}");
                    return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
                }
                catch (Exception ex)
                {
                    // Log error and return 500 response
                    _logger.LogCritical($"Error: {ex.Message}");
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

    }
}
