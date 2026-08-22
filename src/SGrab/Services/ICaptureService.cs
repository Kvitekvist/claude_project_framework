using SGrab.Models;

namespace SGrab.Services;

/// <summary>
/// Starts an interactive region-select screen capture. When the user completes
/// a selection, <see cref="CaptureCompleted"/> is raised with the captured
/// image; cancelling (Esc or a zero-size selection) raises nothing.
/// </summary>
public interface ICaptureService
{
    /// <summary>Raised on the UI thread when a capture is successfully taken.</summary>
    event EventHandler<CapturedImage>? CaptureCompleted;

    /// <summary>Shows the selection overlay and captures the chosen region.</summary>
    Task StartCaptureAsync();
}
