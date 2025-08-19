
using Psycheflow.Api.Contexts;
using Psycheflow.Api.Entities;

namespace Psycheflow.Api.Seeders
{
    public class UserSeeder : ISeeder
    {
        private AppDbContext Context { get; set; }

        public bool onlyHomolog => true;

        public UserSeeder(AppDbContext context)
        {
            Context = context;
        }
        public Task Up()
        {
            User user = new User
            {
                Id = "149e039e-e59e-46db-8b5c-e8294ae4a69f",
                UserName = "Test",
                Email = "test@gmail.com",
                NormalizedEmail = "test@gmail.com",
                CompanyId = new Guid("149e039e-e59e-46db-8b5c-e8294ae4a69f"),
                BirthDate = DateTime.Now,
                PasswordHash = "AQAAAAIAAYagAAAAEJzKBFfdRz3qSX7whKQYjlaNBlskzX6Tnena89xHax78SZWeYc+/pO8rR8wsl19bpg==",
                
                


            };
        }
        public Task Down()
        {
            throw new NotImplementedException();
        }
    }
}
