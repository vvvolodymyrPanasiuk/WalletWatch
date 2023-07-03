using System.ComponentModel.DataAnnotations;

namespace WalletWatch.WebAPI.Models.AuthenticationModels.Request
{
    /// <summary>
    /// Register request object.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// User first name.
        /// </summary>
        /// <remarks>
        /// The first name of the user registering.
        /// </remarks>
        /// <example>
        /// John
        /// </example>
        [Required(ErrorMessage = "The First Name field is required.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        /// <summary>
        /// User last name.
        /// </summary>
        /// <remarks>
        /// The last name of the user registering.
        /// </remarks>
        /// <example>
        /// Doe
        /// </example>
        [Required(ErrorMessage = "The Last Name field is required.")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// User email.
        /// </summary>
        /// <remarks>
        /// The email address of the user registering.
        /// </remarks>
        /// <example>
        /// john.doe@example.com
        /// </example>
        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        /// <summary>
        /// User password.
        /// </summary>
        /// <remarks>
        /// The password of the user registering.
        /// </remarks>
        /// <example>
        /// KGFPA:na15pcD}uCRbjO+!_1
        /// </example>
        [Required(ErrorMessage = "The Password field is required.")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z]).{8,}$", ErrorMessage = "The Password must contain at least 8 characters and one digit character.")]
        public string Password { get; set; }

        /// <summary>
        /// User password confirmation.
        /// </summary>
        /// <remarks>
        /// The confirmation password of the user registering.
        /// </remarks>
        /// <example>
        /// KGFPA:na15pcD}uCRbjO+!_1
        /// </example>
        [Required(ErrorMessage = "The Confirm Password field is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The Password and Confirm Password fields do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
