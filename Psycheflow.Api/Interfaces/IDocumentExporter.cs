
using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Interfaces
{
    public interface IDocumentExporter
    {
        public Task<byte[]> Export(Document document, ExportFormat format);
    }
}
