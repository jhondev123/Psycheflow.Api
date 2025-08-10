
using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Interfaces
{
    public interface IDocumentExporter
    {
        public Task<byte[]> Export(object data, Document document);
    }
}
