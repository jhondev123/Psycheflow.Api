using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;

namespace Psycheflow.Api.Interfaces.ReportExporteres
{
    public abstract class BaseExporter : IBaseExporter
    {
        public abstract ExportFormatEnum Format { get; }

        public async Task<byte[]> Export(Document document)
        {
            ValidateDocument(document);
            return await GenerateExport(document);
        }

        protected virtual void ValidateDocument(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            if (string.IsNullOrEmpty(document.Name))
            {
                throw new ArgumentException("Document name is required.");
            }
        }

        protected abstract Task<byte[]> GenerateExport(Document document);
    }

}
