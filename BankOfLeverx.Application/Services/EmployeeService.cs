using BankOfLeverx.Application.Interfaces;
using BankOfLeverx.Core.DTO;
using BankOfLeverx.Domain.Models;
using BankOfLeverx.Infrastructure.Data.Repositories;

namespace BankOfLeverx.Application.Services
{

    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Employee>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Employee?> GetByIdAsync(int key)
        {
            return _repository.GetByIdAsync(key);
        }

        public async Task<Employee> CreateAsync(EmployeeDTO dto)
        {
            var employee = new Employee
            {
                Name = dto.Name,
                Surname = dto.Surname,
                Position = dto.Position,
                Branch = dto.Branch
            };

            return await _repository.CreateAsync(employee);
        }

        public async Task<Employee?> UpdateAsync(int key, EmployeeDTO dto)
        {
            var employee = new Employee
            {
                Key = key,
                Name = dto.Name,
                Surname = dto.Surname,
                Position = dto.Position,
                Branch = dto.Branch
            };
            var emp = await _repository.UpdateAsync(employee);
            if (emp is null)
            {
                throw new KeyNotFoundException();
            }

            return emp;
        }

        public async Task<Employee?> PatchAsync(int key, EmployeePatchDTO dto)
        {
            var employee = await _repository.GetByIdAsync(key);
            if (employee is null)
                throw new KeyNotFoundException();

            if (dto.Name is not null)
                employee.Name = dto.Name;
            if (dto.Surname is not null)
                employee.Surname = dto.Surname;
            if (dto.Position is not null)
                employee.Position = (int)dto.Position;
            if (dto.Branch is not null)
                employee.Branch = (int)dto.Branch;

            return await _repository.UpdateAsync(employee);
        }

        public Task<bool> DeleteAsync(int key)
        {
            return _repository.DeleteAsync(key);
        }
    }
}
