namespace Psycheflow.Api.Entities.Configs
{
    public class Config : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Company? Company { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public string Key { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }

        public ConfigAi? ConfigAi { get; set; }

    }
}
