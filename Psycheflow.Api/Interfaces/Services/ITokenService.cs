using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Interfaces.Services
{
    public interface ITokenService
    {
        public string GenerateToken(User user,List<string> roles);
    }
}
