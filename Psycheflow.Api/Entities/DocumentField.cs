namespace Psycheflow.Api.Entities
{
    public class DocumentField : BaseEntity
    {
        public Document Document { get; set; }
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public int FieldType { get; set; }
        public int Order { get; set; }
        public bool IsRequired { get; set; }
        public string DefaultValue { get; set; }
        public DocumentField() { }
    }
}
