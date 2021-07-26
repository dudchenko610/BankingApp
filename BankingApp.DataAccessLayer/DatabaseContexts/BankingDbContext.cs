using BankingApp.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.DataAccessLayer.DatabaseContexts
{
    public class BankingDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<MonthlyPayment> MonthlyPayments { get; set; }

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
