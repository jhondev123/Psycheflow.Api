namespace Psycheflow.Api.Dtos.Requests.Document
{
    public class GenerateDocumentRequestDto
    {
        public Guid? DocumentId { get; set; } = null;
        public int DocumentFormat { get; set; } = 1;

    }
}
