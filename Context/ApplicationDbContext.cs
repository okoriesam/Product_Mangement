using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Services.Account;
using Product_Management.Models;

namespace Product_Management.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; } 
        public DbSet<Users> Users{ get; set; }
    }
}
