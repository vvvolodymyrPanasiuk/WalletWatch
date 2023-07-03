using System.ComponentModel.DataAnnotations;

namespace WalletWatch.WebAPI.Models.AuthenticationModels.Request
{
    /// <summary>
    /// Login request object.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// User email.
        /// </summary>
        /// <remarks>
        /// The email address of the user trying to log in.
        /// </remarks>
        /// <example>
        /// john.doe@example.com
        /// </example>
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email address.")]
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        /// <remarks>
        /// The password of the user trying to log in.
        /// </remarks>
        /// <example>
        /// KGFPA:na15pcD}uCRbjO+!_1
        /// </example>
        [Required(ErrorMessage = "The Password field is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
