using CloudSalesAPI.Services.AdministrationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CloudSalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAdministrationService _administration;

        public AccountController(IAdministrationService administration)
        {
            _administration = administration;
        }

        // GET: api/<AccountController>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CustomerAccount>>> GetAll(int customerId)
        {
            var accounts = await _administration.GetAccounts(customerId);
            return Ok(accounts);
        }

        // GET: api/<AccountController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerAccount>> Get(int id)
        {
            var account = await _administration.GetAccountById(id);
            if (account == null) {
                return NotFound();
            }
            return Ok(account);
        }

        // POST api/<AccountController>
        [HttpPost]
        public async Task<ActionResult> Post(int customerId, string accountName)
        {
            try
            {
                await _administration.CreateAccount(customerId, accountName);
                return Ok("Account created successfully");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                // Return a generic error response
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // PUT api/<AccountController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            try
            {
                await _administration.UpdateAccount(id, value);
                return Ok("Account updated");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        // DELETE api/<AccountController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _administration.DeleteAccount(id);
                return Ok("Account updated");
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
