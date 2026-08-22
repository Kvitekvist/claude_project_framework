using System.Collections.ObjectModel;
using System.Windows.Input;
using SGrab.Common;
using SGrab.Models;
using SGrab.Services;

namespace SGrab.ViewModels;

/// <summary>View model for the main window: the capture command and the history filmstrip.</summary>
public sealed class MainViewModel : ViewModelBase
{
    private readonly ICaptureService _capture;
    private readonly IScreenshotStore _store;

    public MainViewModel(ICaptureService capture, IScreenshotStore store)
    {
        _capture = capture;
        _store = store;
        NewCaptureCommand = new RelayCommand(_capture.StartCaptureAsync);

        _store.Changed += (_, _) => Refresh();
        Refresh();
    }

    /// <summary>Starts a new screen capture.</summary>
    public ICommand NewCaptureCommand { get; }

    /// <summary>Past screenshots, newest first, for the filmstrip.</summary>
    public ObservableCollection<Screenshot> Screenshots { get; } = new();

    public bool HasScreenshots => Screenshots.Count > 0;

    private void Refresh()
    {
        var dispatcher = System.Windows.Application.Current?.Dispatcher;
        if (dispatcher is not null && !dispatcher.CheckAccess())
        {
            dispatcher.Invoke(Apply);
        }
        else
        {
            Apply();
        }

        void Apply()
        {
            Screenshots.Clear();
            foreach (var screenshot in _store.Items)
            {
                Screenshots.Add(screenshot);
            }

            OnPropertyChanged(nameof(HasScreenshots));
        }
    }
}
