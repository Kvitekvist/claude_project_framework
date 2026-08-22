using System.Windows;
using System.Windows.Media.Imaging;
using SGrab.Controls;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ModifierKeys = System.Windows.Input.ModifierKeys;

namespace SGrab.Views;

public partial class EditorWindow : Window
{
    public EditorWindow(BitmapSource image, string? title = null)
    {
        InitializeComponent();

        if (!string.IsNullOrEmpty(title))
        {
            Title = $"SGrab Editor — {title}";
        }

        Canvas.Image = image;
        Canvas.SelectionChanged += (_, _) => UpdateButtons();
        Canvas.UndoStackChanged += (_, _) => UpdateButtons();
        UpdateButtons();
    }

    private void OnToolClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: string tag }
            && Enum.TryParse<AnnotationTool>(tag, out var tool))
        {
            Canvas.ActiveTool = tool;
        }
    }

    private void OnUndo(object sender, RoutedEventArgs e) => Canvas.Undo.Undo();

    private void OnRedo(object sender, RoutedEventArgs e) => Canvas.Undo.Redo();

    private void OnDelete(object sender, RoutedEventArgs e) => Canvas.DeleteSelected();

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        base.OnPreviewKeyDown(e);
        if ((e.KeyboardDevice.Modifiers & ModifierKeys.Control) == 0)
        {
            return;
        }

        switch (e.Key)
        {
            case System.Windows.Input.Key.Z:
                Canvas.Undo.Undo();
                e.Handled = true;
                break;
            case System.Windows.Input.Key.Y:
                Canvas.Undo.Redo();
                e.Handled = true;
                break;
        }
    }

    private void UpdateButtons()
    {
        BtnUndo.IsEnabled = Canvas.Undo.CanUndo;
        BtnRedo.IsEnabled = Canvas.Undo.CanRedo;
        BtnDelete.IsEnabled = Canvas.Selected is not null;
    }
}
