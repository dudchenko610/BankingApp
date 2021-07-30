using BankingApp.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.DataAccessLayer.DatabaseContexts
{
    /// <summary>
    /// Gives interface to work with database.
    /// </summary>
    public class BankingDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        /// <summary>
        /// Represents table of deposits.
        /// </summary>
        public DbSet<Deposit> Deposits { get; set; }

        /// <summary>
        /// Represents table of monthly payments.
        /// </summary>
        public DbSet<MonthlyPayment> MonthlyPayments { get; set; }

        /// <summary>
        /// Creates instance of <see cref="BankingDbContext"/>.
        /// </summary>
        /// <param name="options">Specifies database connection options.</param>
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MonthlyPayment>().HasOne(x => x.Deposit)
               .WithMany(x => x.MonthlyPayments)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
