using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Dtos.Requests.Document;
using Psycheflow.Api.Dtos.Requests.Document.CreateDocument;
using Psycheflow.Api.Dtos.Responses;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;
using Psycheflow.Api.Interfaces;
using Psycheflow.Api.Utils;

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

        [HttpPost("")]
        public async Task<IActionResult> CreateDocument([FromBody] CreateDocumentRequestDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                return BadRequest(GenericResponseDto<object>.ToFail($"O parametro {nameof(dto.Name)} é obrigatório"));
            }
            if (string.IsNullOrEmpty(dto.TemplateName))
            {
                return BadRequest(GenericResponseDto<object>.ToFail($"O parametro {nameof(dto.TemplateName)} é obrigatório"));
            }
            Document document = new Document
            {
                Name = dto.Name,
                Description = dto.Description,
                TemplateName = dto.TemplateName,
                CompanyId = dto.CompanyId,
                Fields = dto.Fields.Select(f => new DocumentField
                {
                    Name = f.Name,
                    IsRequired = f.IsRequired,
                    DefaultValue = f.DefaultValue
                }).ToList()
            };
            try
            {
                await Context.Database.BeginTransactionAsync();

                await Context.Documents.AddAsync(document);
                await Context.SaveChangesAsync();
                await Context.Database.CommitTransactionAsync();
                return Ok(GenericResponseDto<object>.ToSuccess("Documento cadastrado com sucesso", document));
            }
            catch (Exception ex)
            {
                await Context.Database.RollbackTransactionAsync();
                return BadRequest(GenericResponseDto<object>.ToException(ex));
            }
        }

        [HttpPost("Generate")]
        public async Task<IActionResult> GenerateDocument([FromBody] GenerateDocumentRequestDto dto)
        {
            try
            {
                if (dto.DocumentId == null)
                {
                    return BadRequest(GenericResponseDto<object>.ToFail("O parametro DocumentId não foi encontrado"));
                }
                Document? document = await Context.Documents
                    .Include(d => d.Fields)
                    .FirstOrDefaultAsync(d => d.Id == dto.DocumentId);

                if (document == null)
                {
                    return BadRequest(GenericResponseDto<object>.ToFail($"Document {dto.DocumentId} não encontrado"));
                }
                foreach (DocumentField field in document.Fields)
                {
                    bool hasParameter = dto.Parameters.ContainsKey(field.Name);
                    bool hasDefaultValue = field.DefaultValue != null;
                    string? parameterValue = hasParameter ? dto.Parameters[field.Name] : null;

                    if (field.IsRequired)
                    {
                        if (hasParameter)
                        {
                            field.Value = parameterValue!;
                            continue;
                        }

                        if (hasDefaultValue)
                        {
                            field.Value = field.DefaultValue!;
                            continue;
                        }

                        throw new Exception($"O parâmetro '{field.Name}' é obrigatório para gerar o documento e não foi encontrado.");
                    }

                    if (hasParameter)
                    {
                        field.Value = parameterValue!;
                        continue;
                    }
                    if (hasDefaultValue)
                    {
                        field.Value = field.DefaultValue!;
                    }
                }
                byte[] documentGenerated = await DocumentExporter.Export(document);
                return File(documentGenerated, "application/pdf", $"{document.Name}.pdf");

            }
            catch (Exception ex)
            {
                return BadRequest(GenericResponseDto<object>.ToException(ex));
            }
        }

        [HttpGet("")]
        public async Task<IActionResult> SearchDocuments()
        {
            User? requestUser = await GetUserRequester.Execute(Context, this);
            if (requestUser == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            List<Document> documents = await Context.Documents.Where(x => x.CompanyId == requestUser.CompanyId || x.CompanyId == null)
                .Include(x => x.Fields)
                .ToListAsync();

            return Ok(GenericResponseDto<List<Document>>.ToSuccess("Documentos encontrados", documents));
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDocumentById([FromRoute] Guid id)
        {
            Document? document = await Context.Documents.Include(x => x.Fields).FirstOrDefaultAsync(x => x.Id == id);
            if (document is null)
            {
                return BadRequest(GenericResponseDto<object>.ToFail($"Documento com o Id {id} não encontrado"));
            }
            return Ok(GenericResponseDto<Document>.ToSuccess("Documento encontrado", document));
        }
    }
}
