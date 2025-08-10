using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using Psycheflow.Api.Entities;
using Psycheflow.Api.Interfaces;

namespace Psycheflow.Api.Services
{
    public class DocumentExporter : IDocumentExporter
    {
        public string DocumentPath { get; set; }
        public IConfiguration Configuration { get; set; }
        public DocumentExporter(IConfiguration configuration)
        {
            DocumentPath = Path.Combine(Directory.GetCurrentDirectory(), $@"Documents\");
            Configuration = configuration;
        }

        public async Task<byte[]> Export(object data, Document document)
        {
            DocumentPath = Path.Combine(DocumentPath, $@"{document.TemplateName}");

            Report report = new Report();
            report.Load(DocumentPath);

            LoadDataSourcesConnection(report);

            report.Prepare();

            ValidateReportParameters(report, document);

            LoadDocumentParameters(report, document);

            using MemoryStream memoryStream = new MemoryStream();
            using PDFSimpleExport pdfExport = new PDFSimpleExport();

            report.Export(pdfExport, memoryStream);

            return memoryStream.ToArray();
        }


        private void LoadDataSourcesConnection(Report report)
        {
            string? connectionString = Configuration["ConnectionStrings:SqlServer"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Erro ao buscar as Connection string");
            }

            foreach (DataConnectionBase connection in report.Dictionary.Connections)
            {
                if (connection is MsSqlDataConnection msConn)
                {
                    msConn.ConnectionString = connectionString;
                }
            }
        }

        private void LoadDocumentParameters(Report report, Document document)
        {
            foreach (DocumentField field in document.Fields.OrderBy(x => x.Order))
            {
                report.SetParameterValue(field.Name, field.Value);
            }
        }
        private void ValidateReportParameters(Report report, Document document)
        {
            ParameterCollection reportParams = report.Parameters;
            int totalParamsInReport = reportParams.Count;

            int totalParamsProvided = document.Fields.Count;

            if (totalParamsInReport != totalParamsProvided)
            {
                throw new Exception($"Número de parâmetros não bate: relatório exige {totalParamsInReport}, mas foram enviados {totalParamsProvided}.");
            }

            foreach (Parameter param in reportParams)
            {
                bool found = document.Fields.Any(f => f.Name.Equals(param.Name, StringComparison.OrdinalIgnoreCase));
                if (!found)
                {
                    throw new Exception($"Parâmetro obrigatório '{param.Name}' não foi informado.");
                }
            }

            foreach (DocumentField field in document.Fields.Where(x => x.IsRequired == true))
            {
                if (reportParams.FindByName(field.Name) == null)
                {
                    throw new Exception($"Parâmetro obrigatório '{field.Name}' não foi informado.");
                }
            }
        }
    }
}
