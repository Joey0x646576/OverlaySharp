using System.ComponentModel.DataAnnotations;

namespace OverlaySharp.Runner.Options
{
    public record RunnerConfiguration
    {
        [Required]
        public string ProcessName { get; set; } = string.Empty;
    }
}
