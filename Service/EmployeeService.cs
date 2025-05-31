using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service;

internal sealed class EmployeeService : IEmployeeService
{

    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public EmployeeService(IRepositoryManager repositoryManager, ILoggerManager logger, IMapper mapper)
    {
        _repository = repositoryManager;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)>  GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId, trackChanges);
        var employeesWithMetaData = await _repository.Employee
        .GetEmployeesAsync(companyId, employeeParameters, trackChanges);
        var employeesDto =
        _mapper.Map<IEnumerable<EmployeeDto>>(employeesWithMetaData);
        return (employees: employeesDto, metaData: employeesWithMetaData.MetaData);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)
    {
        _ = _repository.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeDb = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        var employee = _mapper.Map<EmployeeDto>(employeeDb);

        return employee;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
        _ = await _repository.Company.GetCompanyAsync(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repository.SaveAsync();
        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public async Task DeleteEmployeeForCompanyAsync(Guid companyId, Guid id, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var employeeForCompany = await GetEmployeeForCompanyAndCheckIfExists(companyId, id, trackChanges);
        if (employeeForCompany is null)
            throw new EmployeeNotFoundException(id);
        _repository.Employee.DeleteEmployee(employeeForCompany);
        await _repository.SaveAsync();
    }

    public async Task UpdateEmployeeForCompanyAsync(
        Guid companyId,
        Guid id,
        EmployeeForUpdateDto employeeForUpdate,
        bool compTrackChanges,
        bool empTrackChanges)
    {
        _ = await _repository.Company
        .GetCompanyAsync(companyId, compTrackChanges) ?? throw new CompanyNotFoundException(companyId);

        var employeeEntity = await _repository.Employee
        .GetEmployeeAsync(companyId, id, empTrackChanges) ?? throw new EmployeeNotFoundException(id);

        _mapper.Map(employeeForUpdate, employeeEntity);
        await _repository.SaveAsync();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync (
        Guid companyId, 
        Guid id, 
        bool compTrackChanges, 
        bool empTrackChanges)
    {
        _ = _repository.Company
            .GetCompanyAsync(companyId, compTrackChanges) ?? throw new CompanyNotFoundException(companyId);

        var employeeEntity = await GetEmployeeForCompanyAndCheckIfExists(companyId, id, empTrackChanges);

        var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        return (employeeToPatch, employeeEntity);
    }

    public void SaveChangesForPatch(
        EmployeeForUpdateDto employeeToPatch, 
        Employee employeeEntity)
    {
        _mapper.Map(employeeToPatch, employeeEntity);
        _repository.SaveAsync();
    }

    private async Task<Employee> GetEmployeeForCompanyAndCheckIfExists(Guid companyId, Guid id, bool trackChanges) 
    {
        var employee = await _repository.Employee.GetEmployeeAsync(companyId, id, trackChanges);
        if (employee is null) 
        {
            throw new EmployeeNotFoundException(id);
        }

        return employee;
    }

    private async Task CheckIfCompanyExists(Guid companyId, bool trackChanges)
    {
        var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);
        if (company is null)
        {
            throw new CompanyNotFoundException(companyId);
        }
            
    }
}
