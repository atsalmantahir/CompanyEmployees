using Entities.Models;

namespace Contracts;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges);

    Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);

    Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges);

    void CreateCompany(Company company);

    void DeleteCompany(Company company);
}
