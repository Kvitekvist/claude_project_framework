using System.Windows;
using System.Windows.Input;
using SGrab.Common;
using SGrab.Models;
using SGrab.Services;
using SGrab.ViewModels;
using MouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;

namespace SGrab.Views;

public partial class MainWindow : Window
{
    private readonly IScreenshotStore _store;

    public MainWindow(MainViewModel viewModel, IScreenshotStore store)
    {
        InitializeComponent();
        DataContext = viewModel;
        _store = store;
    }

    private void OnThumbnailClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is FrameworkElement { DataContext: Screenshot shot })
        {
            OpenInEditor(shot);
        }
    }

    private void OnThumbnailDelete(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { DataContext: Screenshot shot })
        {
            return;
        }

        var result = System.Windows.MessageBox.Show(
            this, "Delete this screenshot?", "SGrab",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            _store.Delete(shot.Id);
        }
    }

    private void OpenInEditor(Screenshot shot)
    {
        try
        {
            var image = ImageInterop.LoadFromFile(shot.ImagePath);
            new EditorWindow(image, shot.Id).Show();
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(this, $"Could not open screenshot: {ex.Message}", "SGrab");
        }
    }
}
