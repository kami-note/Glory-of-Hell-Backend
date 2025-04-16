using Microsoft.EntityFrameworkCore;
using Black_Magic_Backend.Models;
using System.Collections.Generic;

namespace Black_Magic_Backend
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Character> Characters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }

        public static void EnsureCreated(ApplicationDbContext context)
        {
            context.Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>().OwnsOne(c => c.Position);
        }
    }
}
