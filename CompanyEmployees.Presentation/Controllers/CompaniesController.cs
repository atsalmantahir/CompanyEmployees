using CompanyEmployees.Presentation.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace CompanyEmployees.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CompaniesController : ControllerBase
{
    private readonly IServiceManager _service;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="service"></param>
    public CompaniesController(IServiceManager service)
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
    }

    /// <summary>
    /// create company
    /// </summary>
    /// <param name="company"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDTO company)
    {
        if (company is null)
            return BadRequest("CompanyForCreationDto object is null");
        var createdCompany = _service.CompanyService.CreateCompany(company);
        return CreatedAtRoute("CompanyById", new { id = createdCompany.Id },
        createdCompany);
    }

    /// <summary>
    /// CompanyCollection
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpGet("collection/({ids})", Name = "CompanyCollection")]
    public IActionResult GetCompanyCollection([ModelBinder(BinderType =
                                                typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
    {
        var companies = _service.CompanyService.GetByIds(ids, trackChanges: false);
        return Ok(companies);
    }



    /// <summary>
    /// get companies by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _service.CompanyService.GetCompany(id, trackChanges: false);
        return Ok(company);
    }


}
