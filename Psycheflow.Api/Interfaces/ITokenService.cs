using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Interfaces
{
    public interface ITokenService
    {
        public string GenerateToken(User user);
    }
}
