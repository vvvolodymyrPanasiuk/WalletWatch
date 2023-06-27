using WalletWatch.Domain.Entities.UserAggregate;

namespace WalletWatch.Domain.Repositories.UserRepository
{
    //Used UserManager<TUser> Class for working with User entyties
    /// <summary>
    /// Interface inherited from IRepository for working with the Users table in the database
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// A method that returns a user from the database by the user's name
        /// </summary>
        /// <param name="firstName">User`s first name</param>
        /// <param name="lastName">User`s last name</param>
        /// <returns>User entity from the database</returns>
        //Task<User> GetUserByUserNameAsync(string firstName, string lastName);

        /// <summary>
        /// A method that checks if the email exists in the database
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>True if email exists, false otherwise</returns>
        //Task<bool> IsExistEmailAsync(string email);

        /// <summary>
        /// A method that finds a user by email in the database
        /// </summary>
        /// <param name="email">Email to search for</param>
        /// <returns>User entity from the database</returns>
        //Task<User> FindByEmailAsync(string email);
    }
}
