global using Microsoft.EntityFrameworkCore;

namespace CloudSalesAPI.Data
{
    public class DataContext : DbContext, IDataContext
    {

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=cloudsalesdb;Trusted_Connection=true;TrustServerCertificate=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PurchasedProduct>()
                .HasIndex(pp => new { pp.ExternalId, pp.CustomerAccountId })
                .IsUnique();
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<PurchasedProduct> PurchasedProducts { get; set; }
    }


}
