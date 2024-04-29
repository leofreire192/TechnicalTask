using CustomersAPI.Models;
using CustomersAPI.Models.Requests;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;

namespace CustomersAPI.Services
{
    public interface ICustomerService
    {
        Task<Result<IEnumerable<Customer>>> GetAllAsync();
        Task<Result<Customer>> GetAsync(Guid id);
        Task<Result<Customer>> CreateAsync(CustomerCreateRequest request);
        Task<Result<Customer>> UpdateAsync(Guid id, CustomerUpdateRequest request);
        Task<Result<Customer>> PatchAsync(Guid id, JsonPatchDocument<Customer> customerModel);
        Task<Result<bool>> Delete(Guid id);
    }
}