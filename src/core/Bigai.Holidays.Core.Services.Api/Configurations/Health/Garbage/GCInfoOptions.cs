namespace Bigai.Holidays.Core.Services.Api.Configurations.Health.Garbage
{
    /// <summary>
    /// Represents the memory limit in bytes for health check.
    /// </summary>
    public class GCInfoOptions
    {
        /// <summary>
        /// The failure threshold (in bytes). 
        /// </summary>
        public long Threshold { get; set; } = 1024L * 1024L * 1024L;
    }
}
