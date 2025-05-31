using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext)
    : base(repositoryContext)
    {
    }

    public async Task<Employee> GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return await FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges)
        .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Employee>> GetEmployees(Guid companyId, bool trackChanges)
    {
        return await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanges)
         .OrderBy(e => e.Name).ToListAsync();
    }

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee) { Delete(employee); }
}
