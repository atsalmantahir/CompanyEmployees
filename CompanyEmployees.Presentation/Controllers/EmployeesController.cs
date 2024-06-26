﻿using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
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
    [HttpGet("{id:guid}")]
    public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
    {
        var employee = _service.EmployeeService.GetEmployee(companyId, id,
        trackChanges: false);
        return Ok(employee);
    }
}
