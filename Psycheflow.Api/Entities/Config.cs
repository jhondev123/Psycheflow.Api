namespace Psycheflow.Api.Entities
{
    public class Config : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public string Key { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }

    }
}
