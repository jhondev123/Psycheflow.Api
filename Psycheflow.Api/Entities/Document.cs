using Microsoft.Extensions.Primitives;
using Psycheflow.Api.Enums;

namespace Psycheflow.Api.Entities
{
    public class Document : BaseEntity
    {
        public Company? Company { get; set; } = null;
        public Guid? CompanyId { get; set; } = default(Guid?);
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<DocumentField> Fields { get; set; }
        public ICollection<ExportFormat> ExportFormats { get; set; }

        public Document() { }

    }
}
