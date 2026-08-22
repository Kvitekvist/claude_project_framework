namespace SGrab.Services;

/// <summary>
/// Starts an interactive screen capture. The concrete region-select
/// implementation arrives in TICKET-0008; TICKET-0007 ships a stub so the
/// button, hotkey, and tray wiring can be exercised end-to-end.
/// </summary>
public interface ICaptureService
{
    Task StartCaptureAsync();
}
