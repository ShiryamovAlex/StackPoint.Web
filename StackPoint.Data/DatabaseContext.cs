using Microsoft.EntityFrameworkCore;
using StackPoint.Data.Models;

namespace StackPoint.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Organisation> Organisations { get; set; }
    }
}