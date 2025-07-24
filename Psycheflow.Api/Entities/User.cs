using Microsoft.AspNetCore.Identity;

namespace Psycheflow.Api.Entities
{
    public class User : IdentityUser
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
