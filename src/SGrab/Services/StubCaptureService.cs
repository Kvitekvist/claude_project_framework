using System.Windows;

namespace SGrab.Services;

/// <summary>
/// Placeholder capture implementation used until TICKET-0008 replaces it with
/// the real region-select overlay. Confirms the trigger path is wired up.
/// </summary>
public sealed class StubCaptureService : ICaptureService
{
    public Task StartCaptureAsync()
    {
        System.Windows.MessageBox.Show(
            "Capture triggered. Region-select capture is implemented in TICKET-0008.",
            "SGrab",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
        return Task.CompletedTask;
    }
}
