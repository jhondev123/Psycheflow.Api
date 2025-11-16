namespace Psycheflow.Api.Dtos.Requests.Document.CreateDocument
{
    public class CreateDocumentRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TemplateName { get; set; } = string.Empty;
        public Guid? CompanyId { get; set; } = default(Guid?);
        public List<CreateDocumentFieldRequestDto> Fields { get; set; } = new List<CreateDocumentFieldRequestDto>();
    }
}
