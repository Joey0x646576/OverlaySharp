using System.Diagnostics;

namespace OverlaySharp.Runner.Options
{
    public class RunnerConfigurationValidator : IValidateOptions<RunnerConfiguration>
    {
        public ValidateOptionsResult Validate(string? name, RunnerConfiguration options)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(options.ProcessName))
            {
                errors.Add("RunnerConfiguration requires ProcessName to not be null or white space.");
            }

            var process = Process.GetProcessesByName(options.ProcessName).FirstOrDefault();
            if (process == null)
            {
                errors.Add($"No process {options.ProcessName} is running.");
            }

            return errors.Count > 0
                ? ValidateOptionsResult.Fail(string.Join("; ", errors))
                : ValidateOptionsResult.Success;
        }
    }
}
