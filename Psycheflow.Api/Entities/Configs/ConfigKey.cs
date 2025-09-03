namespace Psycheflow.Api.Entities.Configs
{
    public record ConfigKey(string Value)
    {
        public static readonly ConfigKey EnableAI = new("EnableAI");
    }
}
