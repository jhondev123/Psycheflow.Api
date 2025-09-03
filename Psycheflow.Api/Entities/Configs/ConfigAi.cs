using FastReport.Utils;

namespace Psycheflow.Api.Entities.Configs
{
    public class ConfigAi : BaseEntity
    {
        public Guid ConfigId { get; set; }
        public Config Config { get; set; }
        public bool IsEnabled { get; set; }
        public string Provider { get; set; }
        public int MaxTokens { get; set; }
        public decimal Temperature { get; set; }

    }
}
