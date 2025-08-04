using Psycheflow.Api.Enums;
using Psycheflow.Api.Interfaces.ReportExporteres;

namespace Psycheflow.Api.Factories
{
    public class ExporterFactory
    {
        private readonly IEnumerable<IBaseExporter> Exporters;

        public ExporterFactory(IEnumerable<IBaseExporter> exporters)
        {
            Exporters = exporters;
        }

        public IBaseExporter GetExporter(ExportFormatEnum format)
        {
            IBaseExporter? Exporter = Exporters.FirstOrDefault(e => e.Format == format);
            if (Exporter == null)
            {
                throw new NotSupportedException($"Formato {format} não suportado.");
            }
            return Exporter;
        }
    }
}
