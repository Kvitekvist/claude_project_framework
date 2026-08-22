using System.Threading;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SGrab.Models;
using SGrab.Services;
using SGrab.ViewModels;
using SGrab.Views;
using WinForms = System.Windows.Forms;

namespace SGrab;

public partial class App : System.Windows.Application
{
    private const string MutexName = "SGrab.SingleInstance.Mutex";
    private const string ShowEventName = "SGrab.SingleInstance.Show";

    private Mutex? _mutex;
    private EventWaitHandle? _showEvent;
    private IHost? _host;
    private WinForms.NotifyIcon? _trayIcon;
    private MainWindow? _mainWindow;
    private bool _isExiting;

    protected override void OnStartup(StartupEventArgs e)
    {
        _mutex = new Mutex(true, MutexName, out bool createdNew);
        _showEvent = new EventWaitHandle(false, EventResetMode.AutoReset, ShowEventName);

        // A second launch just asks the running instance to surface, then exits.
        if (!createdNew)
        {
            _showEvent.Set();
            Shutdown();
            return;
        }

        base.OnStartup(e);

        _host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<ICaptureService, CaptureService>();
                services.AddSingleton<IHotkeyService, HotkeyService>();
                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindow>();
            })
            .Build();

        var services = _host.Services;

        _mainWindow = services.GetRequiredService<MainWindow>();
        MainWindow = _mainWindow;
        _mainWindow.Closing += OnMainWindowClosing;
        _mainWindow.Show();

        var capture = services.GetRequiredService<ICaptureService>();
        capture.CaptureCompleted += OnCaptureCompleted;

        var hotkeys = services.GetRequiredService<IHotkeyService>();
        hotkeys.Initialize(_mainWindow);
        hotkeys.Register(ModifierKeys.Control | ModifierKeys.Shift, Key.S,
            () => _ = capture.StartCaptureAsync());

        SetupTrayIcon(capture);
        StartShowListener();
    }

    private void SetupTrayIcon(ICaptureService capture)
    {
        var menu = new WinForms.ContextMenuStrip();
        menu.Items.Add("New Capture", null, (_, _) => _ = capture.StartCaptureAsync());
        menu.Items.Add("Show SGrab", null, (_, _) => ShowMainWindow());
        menu.Items.Add(new WinForms.ToolStripSeparator());
        menu.Items.Add("Exit", null, (_, _) => ExitApp());

        _trayIcon = new WinForms.NotifyIcon
        {
            Icon = System.Drawing.SystemIcons.Application,
            Visible = true,
            Text = "SGrab",
            ContextMenuStrip = menu,
        };
        _trayIcon.DoubleClick += (_, _) => ShowMainWindow();
    }

    // Placeholder sink until storage (TICKET-0009) and the editor (TICKET-0010)
    // are built: copy the capture to the clipboard and notify via the tray.
    private void OnCaptureCompleted(object? sender, CapturedImage image)
    {
        try
        {
            WinForms.Clipboard.SetImage(image.Bitmap);
            _trayIcon?.ShowBalloonTip(2500, "SGrab",
                $"Captured {image.Width}×{image.Height} — copied to clipboard.",
                WinForms.ToolTipIcon.Info);
        }
        catch (Exception ex)
        {
            _trayIcon?.ShowBalloonTip(2500, "SGrab",
                $"Capture failed: {ex.Message}", WinForms.ToolTipIcon.Error);
        }
        finally
        {
            image.Dispose();
        }
    }

    /// <summary>Background listener that surfaces the window when a second instance signals.</summary>
    private void StartShowListener()
    {
        var thread = new Thread(() =>
        {
            while (_showEvent!.WaitOne())
            {
                if (_isExiting)
                {
                    return;
                }

                Dispatcher.Invoke(ShowMainWindow);
            }
        })
        {
            IsBackground = true,
            Name = "SGrab.ShowListener",
        };
        thread.Start();
    }

    private void ShowMainWindow()
    {
        if (_mainWindow is null)
        {
            return;
        }

        _mainWindow.Show();
        if (_mainWindow.WindowState == WindowState.Minimized)
        {
            _mainWindow.WindowState = WindowState.Normal;
        }

        _mainWindow.Activate();
        _mainWindow.Topmost = true;
        _mainWindow.Topmost = false;
        _mainWindow.Focus();
    }

    // Closing the window hides it to the tray instead of exiting the app.
    private void OnMainWindowClosing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (_isExiting)
        {
            return;
        }

        e.Cancel = true;
        _mainWindow?.Hide();
    }

    private void ExitApp()
    {
        _isExiting = true;
        _showEvent?.Set(); // release the listener thread
        Shutdown();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayIcon?.Dispose();
        (_host?.Services.GetService<IHotkeyService>())?.Dispose();
        _host?.Dispose();

        if (_mutex is not null)
        {
            try
            {
                _mutex.ReleaseMutex();
            }
            catch (ApplicationException)
            {
                // Not the mutex owner (second-instance path); nothing to release.
            }

            _mutex.Dispose();
        }

        _showEvent?.Dispose();
        base.OnExit(e);
    }
}
