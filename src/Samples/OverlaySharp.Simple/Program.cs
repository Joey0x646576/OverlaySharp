using OverlaySharp.Simple;
using System.Diagnostics;

var process = Process.GetProcessesByName("notepad").FirstOrDefault();
if (process == null)
{
    return;
}

var overlay = new SimpleOverlay(process.MainWindowHandle);

// Stops the overlay after 5 seconds...
Thread.Sleep(5000);
await overlay.StopOverlay();