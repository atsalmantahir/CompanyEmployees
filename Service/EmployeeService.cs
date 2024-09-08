using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using Service.Contracts;
using Shared.DataTransferObjects;

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

    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        _ = _repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeesInCompany = _repository.Employee.GetEmployees(companyId, trackChanges);
        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesInCompany);

        return employeesDto;
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid id, bool trackChanges)
    {
        _ = _repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeDb = _repository.Employee.GetEmployee(companyId, id, trackChanges) ?? throw new EmployeeNotFoundException(id);
        var employee = _mapper.Map<EmployeeDto>(employeeDb);

        return employee;
    }    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeForCreation, bool trackChanges)
    {
        _ = _repository.Company.GetCompany(companyId, trackChanges) ?? throw new CompanyNotFoundException(companyId);
        var employeeEntity = _mapper.Map<Employee>(employeeForCreation);
        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        _repository.Save();
        var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public void DeleteEmployeeForCompany(Guid companyId, Guid id, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);
        if (company is null)
            throw new CompanyNotFoundException(companyId);
        var employeeForCompany = _repository.Employee.GetEmployee(companyId, id,
        trackChanges);
        if (employeeForCompany is null)
            throw new EmployeeNotFoundException(id);
        _repository.Employee.DeleteEmployee(employeeForCompany);
        _repository.Save();
    }    public void UpdateEmployeeForCompany(
        Guid companyId,
        Guid id,
        EmployeeForUpdateDto employeeForUpdate,
        bool compTrackChanges,
        bool empTrackChanges)
    {
        _ = _repository.Company
        .GetCompany(companyId, compTrackChanges) ?? throw new CompanyNotFoundException(companyId);

        var employeeEntity = _repository.Employee
        .GetEmployee(companyId, id, empTrackChanges) ?? throw new EmployeeNotFoundException(id);

        _mapper.Map(employeeForUpdate, employeeEntity);
        _repository.Save();
    }
}
