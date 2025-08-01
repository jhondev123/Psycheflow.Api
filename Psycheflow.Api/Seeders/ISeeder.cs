namespace Psycheflow.Api.Seeders
{
    public interface ISeeder
    {
        public bool onlyHomolog { get; }
        public Task Up();
        public Task Down();
    }
}
