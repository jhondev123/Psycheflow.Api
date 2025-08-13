namespace Psycheflow.Api.Dtos.Requests.Document
{
    public class GenerateDocumentRequestDto
    {
        public Guid? DocumentId { get; set; } = null;
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();

    }
}
