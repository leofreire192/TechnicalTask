using CustomersAPI.Models;
using CustomersAPI.Models.Requests;
using CustomersAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CustomersAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService ?? throw new ArgumentNullException(nameof(customerService));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CustomerCreateRequest request)
        {
            var createdCustomer = await _customerService.CreateAsync(request);
            if (createdCustomer.IsFailed)
            {
                if (createdCustomer.Errors.FirstOrDefault() is CustomError customError)
                    return StatusCode((int)customError.StatusCode, customError.Message);

                return BadRequest("Failed to create customer.");
            }

            return Ok(createdCustomer.ValueOrDefault);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(Guid id)
        {
            var getCustomerAsyncResult = await _customerService.GetAsync(id);
            if (getCustomerAsyncResult.IsFailed)
            {
                if (getCustomerAsyncResult.Errors.FirstOrDefault() is CustomError customError)
                    return StatusCode((int)customError.StatusCode, customError.Message);

                return BadRequest(getCustomerAsyncResult.Errors.FirstOrDefault());
            }

            return Ok(getCustomerAsyncResult.ValueOrDefault);
        }

        [HttpGet()]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var getCustomerAsyncResult = await _customerService.GetAllAsync();
            if (getCustomerAsyncResult.IsFailed)
            {
                if (getCustomerAsyncResult.Errors.FirstOrDefault() is CustomError customError)
                    return StatusCode((int)customError.StatusCode, customError.Message);

                return BadRequest(getCustomerAsyncResult.Errors.FirstOrDefault());
            }

            return Ok(getCustomerAsyncResult.ValueOrDefault);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var deleteCustomerResult = await _customerService.Delete(id);
            if (deleteCustomerResult.IsFailed)
            {
                if (deleteCustomerResult.Errors.FirstOrDefault() is CustomError customError)
                    return StatusCode((int)customError.StatusCode, customError.Message);

                return BadRequest(deleteCustomerResult.Errors.FirstOrDefault());
            }

            return Ok(deleteCustomerResult.ValueOrDefault);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerUpdateRequest request)
        {
            var updatedCustomer = await _customerService.UpdateAsync(id, request);
            if (updatedCustomer.IsFailed)
            {
                if (updatedCustomer.Errors.FirstOrDefault() is CustomError customError)
                    return StatusCode((int)customError.StatusCode, customError.Message);

                return BadRequest(updatedCustomer.Errors.FirstOrDefault());
            }

            return Ok(updatedCustomer.ValueOrDefault);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCustomer([FromBody] JsonPatchDocument<Customer> CustomerModel, Guid id)
        {
            var patchResult = await _customerService.PatchAsync(id, CustomerModel);

            if (patchResult.IsFailed)
            {
                var firstError = patchResult.Errors.FirstOrDefault();
                if (firstError is CustomError customError)
                    return StatusCode((int)customError.StatusCode, customError.Message);

                return BadRequest(firstError);
            }

            return Ok(patchResult.ValueOrDefault);
        }
    }
}
