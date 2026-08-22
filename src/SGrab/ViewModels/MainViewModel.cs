using System.Windows.Input;
using SGrab.Common;
using SGrab.Services;

namespace SGrab.ViewModels;

/// <summary>View model for the main window.</summary>
public sealed class MainViewModel : ViewModelBase
{
    private readonly ICaptureService _capture;

    public MainViewModel(ICaptureService capture)
    {
        _capture = capture;
        NewCaptureCommand = new RelayCommand(_capture.StartCaptureAsync);
    }

    /// <summary>Starts a new screen capture.</summary>
    public ICommand NewCaptureCommand { get; }
}
