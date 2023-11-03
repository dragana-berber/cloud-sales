namespace CloudSalesAPI.Provider
{
    public class MockedCloudComputingProvider : ICloudComputingProvider
    {
        private readonly List<Product> _availableProducts; 

        public MockedCloudComputingProvider()
        {
            _availableProducts = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Office 365",
                    AvailableQuantity = 10,
                    Price = 149.99M
                },
                new Product
                {
                    Id = 2,
                    Name = "Microsoft 365",
                    AvailableQuantity = 8,
                    Price = 199.88M
                },
                new Product
                {
                    Id = 3,
                    Name = "Azure",
                    AvailableQuantity = 20,
                    Price = 99.99M
                },
                    new Product
                {
                    Id = 4,
                    Name = "Dynamics 365",
                    AvailableQuantity = 10,
                    Price = 149.99M
                },
                new Product
                {
                    Id = 5,
                    Name = "Power BI",
                    AvailableQuantity = 28,
                    Price = 199.88M
                },
                new Product
                {
                    Id= 6,
                    Name = "Windows 10 Enterprise",
                    AvailableQuantity = 20,
                    Price = 9.99M
                },
                new Product
                {
                    Id = 7,
                    Name = "Microsoft Defender ATP",
                    AvailableQuantity = 130,
                    Price = 8.99M
                },
                new Product
                {
                    Id = 8,
                    Name = "Microsoft 365 Security",
                    AvailableQuantity = 52,
                    Price = 30M
                },
                new Product
                {
                    Id = 9,
                    Name = "Microsoft Teams",
                    AvailableQuantity = 25,
                    Price = 11.11M
                },
                new Product
                {
                    Id = 10,
                    Name = "SharePoint Online",
                    AvailableQuantity = 40,
                    Price = 20.20M
                },
                new Product
                {
                    Id = 11,
                    Name = "Exchange Online",
                    AvailableQuantity = 100,
                    Price = 30M
                }
            };
        }

        public IEnumerable<Product> GetProducts()
        {
            return _availableProducts;
        }

        public bool ProvisionProduct(int productId, int productQty)
        {
            // If the product with the given productId is found, mock response to true
            if (_availableProducts.FirstOrDefault(p => p.Id == productId) != null)
            {
                return true; // Provisioning successful
            }

            return false; // Product provisioning failed due to insufficient quantity or product not found
        }
    }
}
