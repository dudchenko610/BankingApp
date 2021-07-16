using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.DataAccessLayer.DatabaseContexts
{
    public class BankingDbContext : DbContext
    {
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<MonthlyPayment> MonthlyPayments { get; set; }

        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) 
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonthlyPayment>()
               .HasOne(dhi => dhi.Deposit)
               .WithMany(dh => dh.MonthlyPayments)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
