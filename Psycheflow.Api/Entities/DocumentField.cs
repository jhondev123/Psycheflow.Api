using System.Text.Json.Serialization;

namespace Psycheflow.Api.Entities
{
    public class DocumentField : BaseEntity
    {
        [JsonIgnore]
        public Document Document { get; set; }
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; } = false;
        public string? DefaultValue { get; set; } = null;
        public string Value { get; set; } = string.Empty;

        public DocumentField() { }
    }
}
