using BankOfLeverx.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BankOfLeverx.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Key);
                entity.Property(e => e.Key).ValueGeneratedOnAdd();

            });
        }
    }
}