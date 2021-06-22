using MiniBank.Models;
using Microsoft.EntityFrameworkCore;

namespace MiniBank.Data
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }

        public DbSet<Accounts> Accounts{ get; set; }
        public DbSet<Departments> Departments { get; set; }
    }

    public class SubContext : DbContext
    {
        public SubContext(DbContextOptions<SubContext> options) : base(options)
        {
        }

        public DbSet<Customers> Customers{ get; set; }
        public DbSet<Transactions> Transactions{ get; set; }
    }

}