using Psycheflow.Api.Entities;
using Psycheflow.Api.Enums;
using Psycheflow.Api.Factories;
using Psycheflow.Api.Interfaces;

namespace Psycheflow.Api.Services
{
    public class DocumentExporter : IDocumentExporter
    {
        public DocumentExporter()
        {
        }

        public async Task<byte[]> Export(object data, Document document)
        {
            switch (document.TemplateName)
            {
                case "Test":
                    return await GenerateTestDocument(data);
                default:
                    throw new ArgumentException("Template not found");
            }
        }

        private async Task<byte[]> GenerateTestDocument(object data)
        {
            throw new Exception("");
        }
    }
}
