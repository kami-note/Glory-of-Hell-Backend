using Microsoft.EntityFrameworkCore;
using Black_Magic_Backend.Models;
using System.Collections.Generic;

namespace Black_Magic_Backend {
    public class DatabaseContext : DbContext {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
    }
}
