﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RepositoryContext repositoryContext)
    : base(repositoryContext)
    {
    }


    public async Task<IEnumerable<Company>> GetAllCompaniesAsync(bool trackChanges)
    {
        return await FindAll(trackChanges)
         .OrderBy(c => c.Name)
         .ToListAsync();
    }
    public async Task<IEnumerable<Company>> GetByIdsAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        return await FindByCondition(x => ids.Contains(x.Id), trackChanges)
        .ToListAsync();
    }


    public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        return await FindByCondition(c => c.Id.Equals(companyId), trackChanges)
        .SingleOrDefaultAsync();
    }

    public void CreateCompany(Company company) 
    {
        Create(company); 
    }

    public void DeleteCompany(Company company) => Delete(company);
}

