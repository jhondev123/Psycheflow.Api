using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;
using Psycheflow.Api.Factories;
using Psycheflow.Api.Interfaces;
using Psycheflow.Api.Interfaces.ReportExporteres;

namespace Psycheflow.Api.Services
{
    public class ReportExporter : IReportExporter
    {
        public IPdfExporter PdfExporter { get; set; }
        public ExporterFactory ExporterFactory { get; set; }
        public ReportExporter(ExporterFactory exporterFactory)
        {
            ExporterFactory = exporterFactory;
        }

        public async Task<byte[]> Export(Document document, ExportFormat format)
        {
            ExportFormatEnum exportFormat = (ExportFormatEnum)format.Format;

            IBaseExporter exporter = ExporterFactory.GetExporter(exportFormat);

            return await exporter.Export(document);

        }
    }
}
