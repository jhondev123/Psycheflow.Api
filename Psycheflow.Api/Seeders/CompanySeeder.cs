
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Seeders
{
    public class CompanySeeder : ISeeder
    {
        private AppDbContext Context { get; set; }

        public bool onlyHomolog => true;

        public CompanySeeder(AppDbContext context)
        {
            Context = context;
        }
        public async Task Up()
        {
            Company company = new Company
            {
                Id = new Guid("266E089A-F2C2-40BA-539D-08DDCEF1516A"),
                Name = "teste",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            await Context.AddAsync(company);
            Context.SaveChanges();
        }
        public async Task Down()
        {
            await Context.Companies.ExecuteDeleteAsync();
        }

    }
}
