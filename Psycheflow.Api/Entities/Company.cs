using Psycheflow.Api.Entities.Configs;

namespace Psycheflow.Api.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Session> Sessions { get; set; }

        public ICollection<Config> Configs { get; set; }

        public Company()
        {
        }

        public Company(string name)
        {
            Name = name;
        }
    }
}
