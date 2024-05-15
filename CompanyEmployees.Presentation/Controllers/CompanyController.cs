using Microsoft.AspNetCore.Mvc;
using Service.Contracts;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CompanyController : ControllerBase
{
    private readonly IServiceManager _service;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="service"></param>
    public CompanyController(IServiceManager service)
    {
        _service = service;
    }

    /// <summary>
    /// get companies
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public IActionResult GetCompanies()
    {
        try
        {
            var companies =
            _service.CompanyService.GetAllCompanies(trackChanges: false);
            return Ok(companies);
        }
        catch
        {
            return StatusCode(500, "Internal server error");
        }
    }    /// <summary>
    /// get companies by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>    [HttpGet("{id:guid}")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }
}
