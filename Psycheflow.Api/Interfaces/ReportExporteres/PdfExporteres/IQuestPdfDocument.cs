using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Interfaces.ReportExporteres.PdfExporteres
{
    public interface IQuestPdfDocument
    {
        string TemplateName { get; }
        Task<byte[]> GenerateAsync(Document document);

    }
}
