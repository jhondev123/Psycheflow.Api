using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;
using Psycheflow.Api.Factories;
using Psycheflow.Api.Interfaces.ReportExporteres;
using Psycheflow.Api.Interfaces.ReportExporteres.PdfExporteres;

namespace Psycheflow.Api.Services.Exporters.PdfExporter
{
    public class QuestPdfExporter : BaseExporter, IPdfExporter
    {
        public override ExportFormatEnum Format => ExportFormatEnum.PDF;
        private readonly QuestPdfTemplateFactory TemplateFactory;

        public QuestPdfExporter(QuestPdfTemplateFactory templateFactory)
        {
            TemplateFactory = templateFactory;
        }

        protected override Task<byte[]> GenerateExport(Document document)
        {
            IQuestPdfDocument template = TemplateFactory.GetTemplate(document.Name);
            return template.GenerateAsync(document);
        }
    }
}
