using CloudSalesAPI.Services.ProvisioningService;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CloudSalesAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly IProvisioningService _provisioningService;

        public ProductController(IProvisioningService provisioningService)
        {
            _provisioningService = provisioningService;
        }

        // GET: api/<ProductController>
        [HttpGet]
        public ActionResult<IEnumerable<Product>> Get()
        {
            var products = _provisioningService.GetAvailableProducts();
            return Ok(products);
        }

        // GET api/<ProductController>/provisioning/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Product>>> Get(int accountId)
        {
            var products = await _provisioningService.GetPurchasedProductLicenses(accountId);
            return Ok(products);
        }

        // POST api/products/purchase
        [HttpPost("purchase")]
        public async Task<IActionResult> Post([FromBody]PurchaseProductRequest request)
        {
            try
            {
                var success = await _provisioningService.PurchaseProduct(request.AccountId, request.ExternalProductId, request.Quantity);

                if (success)
                {
                    return Ok("Product purchased successfully.");
                }
                else
                {
                    return BadRequest("Insufficient quantity or product not available.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        // PUT api/products/change-quantity
        [HttpPut("change-quantity")]
        public async Task<IActionResult> Put(int externalProductId, int accountId, int quantity)
        {
            try
            {
                await _provisioningService.ChangeProductLicenseQuantity(accountId, externalProductId, quantity);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        // PUT api/products/extend-license
        [HttpPut("extend-license")]
        public async Task<IActionResult> ExtendProductLicense(int externalProductId, int accountId)
        {
            try
            {
                await _provisioningService.ExtendProductLicense(accountId, externalProductId);
                return Ok("License extended successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");

            }
        }

        // PUT api/products/cancel-license
        [HttpPut("cancel-license")]
        public IActionResult CancelProductLicense(int externalProductId, int accountId)
        {
            try
            {
                _provisioningService.CancelProductLicense(accountId, externalProductId);
                return Ok("License canceled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
