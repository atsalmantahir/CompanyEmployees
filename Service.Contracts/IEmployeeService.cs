﻿using Entities.Models;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;

namespace Service.Contracts;

public interface IEmployeeService
{
    Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(
        Guid companyId,
        EmployeeParameters employeeParameters,
        bool trackChanges);

    Task<EmployeeDto> GetEmployeeAsync(
        Guid companyId, 
        Guid id, 
        bool trackChanges);

    Task<EmployeeDto> CreateEmployeeForCompanyAsync(
        Guid companyId, 
        EmployeeForCreationDto employeeForCreation, 
        bool trackChanges);

    Task DeleteEmployeeForCompanyAsync(
        Guid companyId, 
        Guid id, 
        bool trackChanges);

    Task UpdateEmployeeForCompanyAsync(
        Guid companyId, 
        Guid id,
        EmployeeForUpdateDto employeeForUpdate, 
        bool compTrackChanges, bool empTrackChanges);

    Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(
        Guid companyId, 
        Guid id, 
        bool compTrackChanges, 
        bool empTrackChanges);
    void SaveChangesForPatch(
        EmployeeForUpdateDto employeeToPatch, 
        Employee employeeEntity);
}
