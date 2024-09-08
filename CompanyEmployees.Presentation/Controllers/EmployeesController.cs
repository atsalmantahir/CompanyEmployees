using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/companies/{companyId}/employees")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IServiceManager _service;
    public EmployeesController(IServiceManager service)
    {
        _service = service;
    }

    /// <summary>
    /// get employees by company
    /// </summary>
    /// <param name="companyId"></param>
    /// <returns></returns>
    [HttpGet]
    public IActionResult GetEmployeesByCompany(Guid companyId)
    {
        var employees = _service.EmployeeService.GetEmployees(companyId, trackChanges: false);
        return Ok(employees);
    }

    /// <summary>
    /// get employee by company
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}", Name = "GetEmployeeForCompany")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id,
        trackChanges: false);
        return Ok(employee);
    }

    /// <summary>
    /// create employee for company
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="employee"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
    {
        if (employee is null)
        {
            return BadRequest("EmployeeForCreationDto object is null");
        }

        var employeeToReturn = _service.EmployeeService.CreateEmployeeForCompany(companyId, employee, trackChanges: false);

        return CreatedAtRoute("GetEmployeeForCompany", new
        {
            companyId,
            id = employeeToReturn.Id
        },
        employeeToReturn);
    }    /// <summary>
    /// delete employee
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="id"></param>
    /// <returns></returns>    [HttpDelete("{id:guid}")]
    public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
    {
        _service.EmployeeService.DeleteEmployeeForCompany(companyId, id, trackChanges:
        false);
        return NoContent();
    }    /// <summary>
    /// Update employee
    /// </summary>
    /// <param name="companyId"></param>
    /// <param name="id"></param>
    /// <param name="employee"></param>
    /// <returns></returns>    [HttpPut("{id:guid}")]
    public IActionResult UpdateEmployeeForCompany(
        Guid companyId, 
        Guid id,
        [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee is null)
            return BadRequest("EmployeeForUpdateDto object is null");

        _service.EmployeeService
            .UpdateEmployeeForCompany(
            companyId, 
            id, 
            employee, 
            compTrackChanges: false, 
            empTrackChanges: true);

        return NoContent();
    }

}
