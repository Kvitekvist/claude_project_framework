using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace SGrab.Services;

/// <summary>
/// Global hotkey registration via the Win32 RegisterHotKey API, hooked into a
/// WPF window's message loop. The WPF <see cref="ModifierKeys"/> flags map
/// directly onto the MOD_* values expected by RegisterHotKey.
/// </summary>
public sealed class HotkeyService : IHotkeyService
{
    private const int WmHotkey = 0x0312;

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    private readonly Dictionary<int, Action> _callbacks = new();
    private HwndSource? _source;
    private IntPtr _handle;
    private int _nextId = 1;

    public void Initialize(Window window)
    {
        var helper = new WindowInteropHelper(window);
        helper.EnsureHandle();
        _handle = helper.Handle;
        _source = HwndSource.FromHwnd(_handle);
        _source?.AddHook(WndProc);
    }

    public bool Register(ModifierKeys modifiers, Key key, Action callback)
    {
        if (_handle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Initialize must be called before registering hotkeys.");
        }

        int id = _nextId++;
        uint vk = (uint)KeyInterop.VirtualKeyFromKey(key);
        if (!RegisterHotKey(_handle, id, (uint)modifiers, vk))
        {
            return false;
        }

        _callbacks[id] = callback;
        return true;
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == WmHotkey && _callbacks.TryGetValue(wParam.ToInt32(), out var callback))
        {
            callback();
            handled = true;
        }

        return IntPtr.Zero;
    }

    public void Dispose()
    {
        _source?.RemoveHook(WndProc);
        if (_handle != IntPtr.Zero)
        {
            foreach (var id in _callbacks.Keys)
            {
                UnregisterHotKey(_handle, id);
            }
        }

        _callbacks.Clear();
    }
}
