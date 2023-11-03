using CloudSalesAPI.Data;
using CloudSalesAPI.Models;
using CloudSalesAPI.Provider;
using CloudSalesAPI.Services.ProvisioningService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.EntityFrameworkCore;

namespace CloudSalesAPI.Tests
{
    public class ProvisioningServiceTests
    {
        private readonly ProvisioningService _provisioningService;
        private readonly Mock<ICloudComputingProvider> _providerMock;
        private readonly Mock<IDataContext> _contextMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public ProvisioningServiceTests()
        {
            _providerMock = new Mock<ICloudComputingProvider>();
            _contextMock = new Mock<IDataContext>();
            _configurationMock = new Mock<IConfiguration>();

            _providerMock.Setup(m => m.GetProducts()).Returns(new List<Product> { new Product { Id = 1, Name = "sample product", AvailableQuantity = 100 } });

            _provisioningService = new ProvisioningService(_providerMock.Object, _contextMock.Object, _configurationMock.Object);
        }

        [Fact]
        public async Task CancelProductLicense_WhenValidProductExists_ShouldCancelLicense()
        {
            // Arrange
            var purchasedProduct = new PurchasedProduct { CustomerAccountId = 1, ExternalId = 1, State = State.Active };
            _contextMock.Setup(x => x.PurchasedProducts).ReturnsDbSet(new List<PurchasedProduct> { purchasedProduct });

            // Act
            var result = await _provisioningService.CancelProductLicense(1, 1);

            // Assert
            Assert.True(result);
            Assert.Equal(State.Suspended, purchasedProduct.State);
            _contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task ExtendProductLicense_WhenValidProductExistsAndConfigurationIsValid_ShouldExtendLicense()
        {
            // Arrange
            var purchasedProducts = new List<PurchasedProduct> {
                new PurchasedProduct { CustomerAccountId = 1, ExternalId = 1, State = State.Active, ValidToDate = DateTime.Now }};
            _contextMock.Setup(x => x.PurchasedProducts).ReturnsDbSet(purchasedProducts);
            _configurationMock.Setup(m => m["BillingPeriodMonths"]).Returns("3");

            // Act
            var result = await _provisioningService.ExtendProductLicense(1, 1);

            // Assert
            Assert.True(result);
            _contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
            Assert.Equal(DateTime.Now.AddMonths(3).Date, purchasedProducts.First().ValidToDate.Date);
        }

        [Fact]
        public async Task ChangeProductLicenseQuantity_WhenValidProductAndEnoughAvailableLicenses_ShouldChangeQuantity()
        {
            // Arrange
            var purchasedProduct = new PurchasedProduct { CustomerAccountId = 1, ExternalId = 1, State = State.Active };
            _contextMock.Setup(x => x.PurchasedProducts).ReturnsDbSet(new List<PurchasedProduct> { purchasedProduct });

            // Act
            var result = await _provisioningService.ChangeProductLicenseQuantity(1, 1, 5);

            // Assert
            Assert.True(result);
            Assert.Equal(5, purchasedProduct.Quantity);
            _contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public void GetAvailableProducts_ShouldReturnProductsFromProvider()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Product 1" }, new Product { Id = 2, Name = "Product 2" } };
            _providerMock.Setup(m => m.GetProducts()).Returns(products);

            // Act
            var result = _provisioningService.GetAvailableProducts();

            // Assert
            Assert.Equal(products, result.ToList());
        }

        [Fact]
        public async Task GetPurchasedProductLicenses_WhenValidAccountId_ShouldReturnPurchasedProducts()
        {
            // Arrange
            var accountId = 1;
            var purchasedProducts = new List<PurchasedProduct> { new PurchasedProduct { CustomerAccountId = accountId } };
            _contextMock.Setup(x => x.PurchasedProducts).ReturnsDbSet(purchasedProducts);

            // Act
            var result = await _provisioningService.GetPurchasedProductLicenses(accountId);

            // Assert
            Assert.Equal(purchasedProducts, result);
        }

        [Fact]
        public async Task PurchaseProduct_WhenValidProductAndQuantity_ShouldPurchaseProduct()
        {
            // Arrange
            var productId = 1;
            var quantity = 2;
            var purchasedProduct = new List<PurchasedProduct> {
                new PurchasedProduct { CustomerAccountId = 1, ExternalId = 1, State = State.Active } };
            _contextMock.Setup(x => x.PurchasedProducts).ReturnsDbSet(purchasedProduct);
            _providerMock.Setup(m => m.ProvisionProduct(productId, quantity)).Returns(true);

            _configurationMock.Setup(m => m["BillingPeriodMonths"]).Returns("2");

            // Act
            var result = await _provisioningService.PurchaseProduct(1, productId, quantity);

            // Assert
            Assert.True(result);
            _contextMock.Verify(m => m.PurchasedProducts.AddAsync(It.IsAny<PurchasedProduct>(), default), Times.Once);
            _contextMock.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }
    }
}
