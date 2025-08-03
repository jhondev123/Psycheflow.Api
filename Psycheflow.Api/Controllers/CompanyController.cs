using Microsoft.AspNetCore.Mvc;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Company;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        public AppDbContext Context { get; set; }
        public CompanyController(AppDbContext context) { Context = context; }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyRequestDto requestDto)
        {
            try
            {
                Company company = new Company
                {
                    Name = requestDto.Name,
                };
                await Context.Companies.AddAsync(company);
                await Context.SaveChangesAsync();

                return Ok(GenericResponseDto<Company>.ToSuccess("Company criada com sucesso",company));
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
