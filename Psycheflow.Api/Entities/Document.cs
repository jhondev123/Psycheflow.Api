using Microsoft.Extensions.Primitives;
using Psycheflow.Api.Enums;

namespace Psycheflow.Api.Entities
{
    public class Document : BaseEntity
    {
        public Company? Company { get; set; } = null;
        public Guid? CompanyId { get; set; } = default(Guid?);
        public string TemplateName { get; set; }
        public string Description { get; set; }     
        public Document() { }

    }
}
