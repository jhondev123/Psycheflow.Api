using Psycheflow.Api.Entities;
using Psycheflow.Api.Interfaces.ReportExporteres.PdfExporteres;

namespace Psycheflow.Api.Services.Exporters.PdfExporter.QuestPdfTemplates
{
    public class TestTemplate : IQuestPdfDocument
    {
        public string TemplateName => "Test";

        public Task<byte[]> GenerateAsync(Document document)
        {
            throw new Exception("");
        }
    }
}
