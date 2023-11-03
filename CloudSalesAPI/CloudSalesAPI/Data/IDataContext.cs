namespace CloudSalesAPI.Data
{
    public interface IDataContext
    {
        DbSet<Customer> Customers { get; }
        DbSet<CustomerAccount> CustomerAccounts { get; }
        DbSet<PurchasedProduct> PurchasedProducts { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);


    }

}
