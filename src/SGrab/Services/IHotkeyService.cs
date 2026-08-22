using System.Windows;
using System.Windows.Input;

namespace SGrab.Services;

/// <summary>
/// Registers system-wide global hotkeys and invokes callbacks when they fire.
/// Must be initialised with a window that owns a Win32 handle before use.
/// </summary>
public interface IHotkeyService : IDisposable
{
    /// <summary>Binds the service to a window's message loop.</summary>
    void Initialize(Window window);

    /// <summary>
    /// Registers a global hotkey. Returns false if the combination is already
    /// taken by another application.
    /// </summary>
    bool Register(ModifierKeys modifiers, Key key, Action callback);
}
