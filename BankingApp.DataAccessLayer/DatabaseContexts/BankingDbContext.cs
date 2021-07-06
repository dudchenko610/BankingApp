using BankingApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.DataAccessLayer.DatabaseContexts
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DepositeHistoryItem>()
               .HasOne(dhi => dhi.DepositeHistory)
               .WithMany(dh => dh.DepositeHistoryItems)
               .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<DepositeHistory> DepositeHistories { get; set; }
        public DbSet<DepositeHistoryItem> DepositeHistoryItems { get; set; }
    }
}
