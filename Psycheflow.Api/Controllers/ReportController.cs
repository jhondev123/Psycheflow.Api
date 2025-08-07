using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Document;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;
using Psycheflow.Api.Interfaces;

namespace Psycheflow.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentController : ControllerBase
    {
        private AppDbContext Context { get; set; }

        private IDocumentExporter DocumentExporter { get; set; }

        public DocumentController(IDocumentExporter exporter, AppDbContext context)
        {
            DocumentExporter = exporter;
            Context = context;

        }

        [HttpPost]
        public async Task<IActionResult> GenerateDocument([FromBody] GenerateDocumentRequestDto dto)
        {
            if (dto.DocumentId == null)
            {
                return BadRequest(GenericResponseDto<object>.ToFail("O parametro DocumentId não foi encontrado"));
            }
            if (!Enum.TryParse<ExportFormatEnum>(dto.DocumentFormat.ToString(), out var exportFormat))
            {
                return BadRequest(GenericResponseDto<object>.ToFail("O parametro DocumentFormat não é um formato válido"));
            }
            Document? document = await Context.Documents.FirstOrDefaultAsync(x => x.Id == dto.DocumentId);
            if (document == null)
            {
                return BadRequest(GenericResponseDto<object>.ToFail($"Document {dto.DocumentId} não encontrado"));
            }


            throw new Exception("");

        }
    }
}
