namespace Psycheflow.Api.Entities
{
    public class Patient : BaseEntity
    {
        public User? User { get; set; }
        public string? UserId { get; set; } = null;
        public Company Company { get; set; }
        public Guid CompanyId { get; set; }

        public ICollection<Session> Sessions { get; set; }

        public Patient()
        {
        }

        public Patient(string? userId, Guid companyId)
        {
            UserId = userId;
            CompanyId = companyId;
        }
        public string GetName()
        {
            return User?.UserName ?? string.Empty;
        }
    }
}
