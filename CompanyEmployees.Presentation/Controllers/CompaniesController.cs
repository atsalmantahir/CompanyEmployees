using CompanyEmployees.Presentation.ActionFilters;
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
    public async Task<IActionResult> GetCompanies()
    {
        try
        {
            var companies =
            await _service.CompanyService.GetAllCompaniesAsync(trackChanges: false);
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
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDTO company)
    {
        var createdCompany = await _service.CompanyService.CreateCompanyAsync(company);
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
        var companies = _service.CompanyService.GetByIdsAsync(ids, trackChanges: false);
        return Ok(companies);
    }

    /// <summary>
    /// Create company collection
    /// </summary>
    /// <param name="companyCollection"></param>
    /// <returns></returns>
    [HttpPost("collection")]
    public async Task<IActionResult> CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDTO> companyCollection)
    {
        var result =
        await _service.CompanyService.CreateCompanyCollectionAsync(companyCollection);
        return CreatedAtRoute("CompanyCollection", new { result.ids },
        result.companies);
    }



    /// <summary>
    /// get companies by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _service.CompanyService.GetCompanyAsync(id, trackChanges: false);
        return Ok(company);
    }


    /// <summary>
    /// Delete company
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        await _service.CompanyService.DeleteCompanyAsync(id, trackChanges: false);
        return NoContent();
    }


    /// <summary>
    /// update company
    /// </summary>
    /// <param name="id"></param>
    /// <param name="company"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(
        Guid id, 
        [FromBody] CompanyForUpdateDto company)
    {
        await _service.CompanyService
            .UpdateCompanyAsync(
            id, 
            company, 
            trackChanges: true);

        return NoContent();
    }


}
