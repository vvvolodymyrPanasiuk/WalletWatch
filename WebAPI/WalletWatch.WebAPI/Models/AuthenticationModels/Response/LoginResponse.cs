using System;

namespace WalletWatch.WebAPI.Models.AuthenticationModels.Response
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool SubscriptionStatus { get; set; }
    }
}
