using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities.Configs;

namespace Psycheflow.Api.Services
{
    public class ConfigService
    {
        private AppDbContext Context { get; set; }
        public ConfigService(AppDbContext context) { Context = context; }

        public async Task<List<Config>> GetAllConfigsByCompanyAndUser(Guid companyId, string userId)
        {
            return await Context.Configs
                .Where(c => c.CompanyId == companyId && c.UserId == userId)
                .Include(c => c.ConfigAi)
                .ToListAsync();
        }
        public async Task<string?> GetConfigValueByKey(Guid companyId, ConfigKey key)
        {
            return await Context.Configs
                .Where(c => c.CompanyId == companyId && c.Key == key.ToString())
                .Select(c => c.Value)
                .FirstOrDefaultAsync();
        }
        public async Task SetConfigValue(Guid companyId, ConfigKey key, string value)
        {
            Config? config = await Context.Configs
                .FirstOrDefaultAsync(c => c.CompanyId == companyId && c.Key == key.ToString());

            if (config == null)
            {
                config = new Config { CompanyId = companyId, Key = key.ToString(), Value = value };
                Context.Configs.Add(config);
            }
            else
            {
                config.Value = value;
            }
            await Context.SaveChangesAsync();
        }
    }
}
