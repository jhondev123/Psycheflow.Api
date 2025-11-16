namespace Psycheflow.Api.Dtos.Requests.Document.CreateDocument
{
    public class CreateDocumentFieldRequestDto
    {
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool IsRequired { get; set; } = false;
        public string? DefaultValue { get; set; } = null;
    }
}
