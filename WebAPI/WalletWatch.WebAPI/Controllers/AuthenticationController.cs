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

namespace WalletWatch.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


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
                        Console.WriteLine($"Failed to register user: A user with this login already exists.", ConsoleColor.Red);
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
                        Console.WriteLine($"Failed to register user {user.UserName}: {errorMessage}", ConsoleColor.Red);
                        throw new Exception(errorMessage);
                    }

                    // If user was created successfully, return status 201
                    return StatusCode(StatusCodes.Status201Created, $"Welcome, {model.FirstName} {model.LastName}!");
                }
                catch (AuthenticationException ex)
                {
                    // Handle authentication exception and return 400 response
                    Console.WriteLine($"Authorization: {ex.Message}", ConsoleColor.Red);
                    return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
                }
                catch (Exception ex)
                {
                    // Log error and return 500 response
                    Console.WriteLine($"Error: {ex.Message}", ConsoleColor.Red);
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }


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
                        Console.WriteLine($"User with login {model.Email} not found", ConsoleColor.Red);
                        throw new AuthenticationException("Invalid login or password");
                    }

                    // Check password
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (!result.Succeeded)
                    {
                        Console.WriteLine($"Failed to authenticate user with login {model.Email}", ConsoleColor.Red);
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
                    Console.WriteLine($"Authorization: {ex.Message}", ConsoleColor.Red);
                    return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
                }
                catch (Exception ex)
                {
                    // Log error and return 500 response
                    Console.WriteLine($"Error: {ex.Message}", ConsoleColor.Red);
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
                }
            }
            return BadRequest(ModelState);
        }

    }
}
