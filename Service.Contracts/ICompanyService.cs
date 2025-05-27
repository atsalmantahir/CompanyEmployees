using Shared.DataTransferObjects;

namespace Service.Contracts;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);

    Task<IEnumerable<CompanyDto>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);

    Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDTO company);

    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDTO> companyCollection);

    Task DeleteCompanyAsync(Guid companyId, bool trackChanges);

    Task UpdateCompanyAsync(
        Guid companyid,
        CompanyForUpdateDto companyForUpdate, 
        bool trackChanges);
}
