using CustomersAPI.Mapper;
using CustomersAPI.Models;
using CustomersAPI.Models.Requests;
using CustomersAPI.Repositories;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections;
using System.Net;
using System.Security.Cryptography.Xml;

namespace CustomersAPI.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository<Customer> _customerRepository;
        public CustomerService(ICustomerRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Customer>> GetAsync(Guid id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<Result<IEnumerable<Customer>>> GetAllAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Result<Customer>> CreateAsync(CustomerCreateRequest request)
        {
            var mapper = new CustomerMapper();
            var customer = mapper.CustomerCreateRequestToCustomer(request);

            return await _customerRepository.AddAsync(customer);
        }

        public async Task<Result<Customer>> UpdateAsync(Guid id, CustomerUpdateRequest request)
        {
            var existingCustomerResult = await _customerRepository.GetByIdAsync(id);
            if (existingCustomerResult.IsFailed)
            {
                return existingCustomerResult;
            }

            var mappper = new CustomerMapper();
            var customer = mappper.CustomerUpdateRequestToCustomer(request);
            customer.Id = id;

            var updateResult = await _customerRepository.UpdateAsync(id,customer);
            return updateResult;
        }

        public async Task<Result<bool>> Delete(Guid id)
        {
            return await _customerRepository.DeleteAsync(id);
        }

        public async Task<Result<Customer>> PatchAsync(Guid id, JsonPatchDocument<Customer> customerModel)
        {
            var existingCustomerResult = await _customerRepository.GetByIdAsync(id);
            if (existingCustomerResult.IsFailed)
            {
                return existingCustomerResult;
            }

            return await _customerRepository.PatchAsync(id, customerModel);
        }
    }
}
