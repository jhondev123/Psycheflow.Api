using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;
using System.Security.Claims;

namespace Psycheflow.Api.Utils
{
    public static class GetUserRequester
    {
        public static async Task<User?> Execute(AppDbContext context, ControllerBase controller)
        {
            Claim? claim = controller.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return null;
            }
            string userId = claim.Value;

            return await context.Users.FirstOrDefaultAsync(x=> x.Id == userId);
        }
    }
}
