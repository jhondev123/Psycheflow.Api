using Microsoft.AspNetCore.Identity;

namespace Psycheflow.Api.Entities
{
    public class User : IdentityUser
    {
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
