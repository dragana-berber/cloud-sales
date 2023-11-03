namespace CloudSalesAPI.Services.ProvisioningService
{
    public interface IProvisioningService
    {

        IEnumerable<Product> GetAvailableProducts();

        // Purchase software license through CCP for a specific account
        Task<bool> PurchaseProduct(int accountId, int productId, int quantity);

        // Get purchased software licenses for a specific account
        Task<List<PurchasedProduct>> GetPurchasedProductLicenses(int accountId);

        // Change quantity of service licenses per subscription for a specific software
        Task<bool> ChangeProductLicenseQuantity(int accountId, int externalProductId, int newQuantity);

        // Cancel the specific software license under any account
        Task<bool> CancelProductLicense(int accountId, int externalProductId);

        // Extend the software license valid to date for a specific product
        Task<bool> ExtendProductLicense(int accountId, int externalProductId);
    }
}
