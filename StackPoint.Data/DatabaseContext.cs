using Microsoft.EntityFrameworkCore;
using StackPoint.Data.Models;

namespace StackPoint.Data
{
    public class DatabaseContext : DbContext
    {
        protected DatabaseContext(){

        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Organisation> Organisations { get; set; }
    }
}