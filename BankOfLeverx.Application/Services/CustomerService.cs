using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;

namespace BankOfLeverx.Application.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int key);
        Task<Customer> CreateAsync(CustomerDTO dto);
        Task<Customer?> UpdateAsync(int key, CustomerDTO dto);
        Task<Customer?> PatchAsync(int key, CustomerPatchDTO dto);
        Task<bool> DeleteAsync(int key);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Customer>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Customer?> GetByIdAsync(int key)
        {
            return _repository.GetByIdAsync(key);
        }

        public async Task<Customer> CreateAsync(CustomerDTO dto)
        {
            var customer = new Customer
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Category = dto.Category,
                Branch = dto.Branch
            };

            return await _repository.CreateAsync(customer);
        }

        public async Task<Customer?> UpdateAsync(int key, CustomerDTO dto)
        {
            var customer = new Customer
            {
                Key = key,
                Name = dto.Name,
                Surname = dto.Surname,
                Category = dto.Category,
                Branch   = dto.Branch
            };

            return await _repository.UpdateAsync(customer);
        }

        public async Task<Customer?> PatchAsync(int key, CustomerPatchDTO dto)
        {
            var customer = await _repository.GetByIdAsync(key);
            if (customer is null)
                return null;

            // Apply patch
            if (dto.Name is not null)
                customer.Name = dto.Name;
            if (dto.Surname is not null)
                customer.Surname = dto.Surname;
            if (dto.Category is not null)
                customer.Category = (int) dto.Category;
            if (dto.Branch is not null)
                customer.Branch = (int)dto.Branch;

            return await _repository.UpdateAsync(customer);
        }

        public Task<bool> DeleteAsync(int key)
        {
            return _repository.DeleteAsync(key);
        }
    }
}
