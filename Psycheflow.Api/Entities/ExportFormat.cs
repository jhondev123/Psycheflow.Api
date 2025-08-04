namespace Psycheflow.Api.Entities
{
    public class ExportFormat : BaseEntity
    {
        public string Name { get; set; }
        public string Extesion { get; set; }
        public int Format {  get; set; }
        public string MimeType { get; set; }
        public Document Document { get; set; }
        public Guid DocumentId { get; set; }
        public ExportFormat() { }
    }
}
