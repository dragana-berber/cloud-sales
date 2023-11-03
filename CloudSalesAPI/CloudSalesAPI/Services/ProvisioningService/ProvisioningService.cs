using CloudSalesAPI.Data;
using CloudSalesAPI.Models;
using CloudSalesAPI.Provider;
using Microsoft.Extensions.Configuration;

namespace CloudSalesAPI.Services.ProvisioningService
{
    public class ProvisioningService : IProvisioningService
    {

        private readonly ICloudComputingProvider _provider;
        private readonly DataContext _context;

        private readonly IConfiguration _configuration;
        public ProvisioningService(ICloudComputingProvider provider, DataContext context, IConfiguration configuration)
        {
            _provider = provider;
            _context = context;
            _configuration = configuration;
        }

 
        public async Task<bool> CancelProductLicense(int accountId, int productId)
        {
            var purchasedProduct = _context.PurchasedProducts
                .FirstOrDefault(pp => pp.CustomerAccountId == accountId && pp.ExternalId == productId && pp.State == State.Active);

            if (purchasedProduct != null)
            {
                purchasedProduct.State = State.Suspended;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> ExtendProductLicense(int accountId, int externalProductId)
        {
            var purchasedProduct = _context.PurchasedProducts
                .FirstOrDefault(pp => pp.CustomerAccountId == accountId && pp.ExternalId == externalProductId && pp.State == State.Active);

            if (purchasedProduct != null)
            {
                var billingMonthsString = _configuration["BillingPeriodMonths"];
                if (int.TryParse(billingMonthsString, out int billingMonths))
                {
                    purchasedProduct.ValidToDate = purchasedProduct.ValidToDate.AddMonths(billingMonths);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    // Handle parsing error, the configuration value is not a valid integer
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> ChangeProductLicenseQuantity(int accountId, int productId, int newQuantity)
        {
            var purchasedProduct = _context.PurchasedProducts
                .FirstOrDefault(pp => pp.CustomerAccountId == accountId && pp.ExternalId == productId && pp.State == State.Active);

            if (purchasedProduct != null)
            {
                // Check with the cloud provider if there are enough available licenses
                var availableProducts = _provider.GetProducts();
                var cloudProduct = availableProducts.FirstOrDefault(p => p.Id == productId);

                if (cloudProduct != null && cloudProduct.AvailableQuantity >= newQuantity)
                {
                    purchasedProduct.Quantity = newQuantity;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<Product> GetAvailableProducts()
        {
            return _provider.GetProducts();
        }

        public async Task<List<PurchasedProduct>> GetPurchasedProductLicenses(int accountId)
        {
            var purchasedProducts = await _context.PurchasedProducts.
                            Where(c => c.CustomerAccountId == accountId)
                               .ToListAsync();
            return purchasedProducts;
        }

        public async Task<bool> PurchaseProduct(int accountId, int productId, int quantity)
        {
            var success = _provider.ProvisionProduct(productId, quantity);
            if (success) {
                var productName = _provider.GetProducts().First(x => x.Id == productId).Name;
                
                DateTime validToDate;
                var billingMonthsString = _configuration["BillingPeriodMonths"];
                if (int.TryParse(billingMonthsString, out int billingMonths))
                {
                    validToDate = DateTime.Now.AddMonths(billingMonths);
                }
                else
                {
                    // Handle parsing error, the configuration value is not a valid integer
                    // take default value 1 month
                    validToDate = DateTime.Now.AddMonths(1);
                }

                await _context.PurchasedProducts.AddAsync(new PurchasedProduct
                {
                    CustomerAccountId = accountId,
                    ExternalId = productId,
                    Name = productName,
                    Quantity = quantity,
                    State = State.Active,
                    ValidToDate = validToDate
                });
                await _context.SaveChangesAsync();

            }
            return success;
        }
    }
}
