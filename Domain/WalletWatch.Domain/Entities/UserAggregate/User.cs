using Microsoft.AspNetCore.Identity;

namespace WalletWatch.Domain.Entities.UserAggregate
{
    /// <summary>
    /// Class represents a user and inherits from IdentityUser
    /// </summary>
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool SubscriptionStatus { get; set; }
    }
}
