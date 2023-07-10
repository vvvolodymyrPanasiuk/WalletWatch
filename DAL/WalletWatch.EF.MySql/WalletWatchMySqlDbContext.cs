using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WalletWatch.Domain.Entities.UserAggregate;

namespace WalletWatch.EF.MySql
{
    public class WalletWatchMySqlDbContext : IdentityDbContext<User>
    {
        public WalletWatchMySqlDbContext(DbContextOptions<WalletWatchMySqlDbContext> options) : base(options) { }
        
        public override DbSet<User> Users { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message), new[] { RelationalEventId.CommandExecuted });
            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
