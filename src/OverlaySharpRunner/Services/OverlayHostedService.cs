﻿using System.Diagnostics;
using OverlaySharp.Runner.Overlay;

namespace OverlaySharp.Runner.Services
{
    internal class OverlayHostedService(
        ILogger<OverlayHostedService> logger,
        IOptions<RunnerConfiguration> runnerOptions)
        : IHostedService
    {
        private readonly RunnerConfiguration _runnerOptions = runnerOptions.Value;
        private OverlayRunner _overlay = null!;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Launching overlay on {ProcessName}", _runnerOptions.ProcessName);

            var process = Process.GetProcessesByName(_runnerOptions.ProcessName).FirstOrDefault();
            _overlay = new OverlayRunner(process!.MainWindowHandle)
            {
                MeasureFps = true
            };

            await _overlay.StartOverlayAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _overlay.StopOverlayAsync();
        }
    }
}
