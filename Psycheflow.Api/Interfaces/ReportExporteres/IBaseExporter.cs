using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;

namespace Psycheflow.Api.Interfaces.ReportExporteres
{
    public interface IBaseExporter
    {
        public ExportFormatEnum Format { get; }
        public Task<byte[]> Export(Document document);
    }
}
