using Application.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Entities.ApplicationContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        public DbSet<Book> Book { get; set; }
        public DbSet<BorrowRecord> BorrowRecord { get; set; }
        public DbSet<ApplicationLogs> ApplicationLogs { get; set; }
    }
}
