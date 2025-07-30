namespace Psycheflow.Api.Seeders
{
    public interface ISeeder
    {
        public Task Up();
        public Task Down();
    }
}
