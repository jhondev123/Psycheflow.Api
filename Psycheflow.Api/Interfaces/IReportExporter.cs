
using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Interfaces
{
    public interface IReportExporter
    {
        public Task<byte[]> Export(Document document, ExportFormat format);
    }
}
