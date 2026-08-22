using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SGrab.Controls;
using SGrab.Models.Annotations;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using ModifierKeys = System.Windows.Input.ModifierKeys;
using Point = System.Windows.Point;
using WinForms = System.Windows.Forms;

namespace SGrab.Views;

public partial class EditorWindow : Window
{
    private static string? _lastDirectory;

    private TextAnnotation? _editing;
    private AnnotationObject? _editBefore;

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
        Canvas.TextEditRequested += OnTextEditRequested;
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

    private void OnColorClick(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement { Tag: string hex }
            && ColorConverter.ConvertFromString(hex) is Color color)
        {
            Canvas.ApplyColorToSelection(color);
        }
    }

    private void OnCustomColor(object sender, RoutedEventArgs e)
    {
        using var dialog = new WinForms.ColorDialog { AnyColor = true, FullOpen = true };
        if (dialog.ShowDialog() == WinForms.DialogResult.OK)
        {
            var c = dialog.Color;
            Canvas.ApplyColorToSelection(Color.FromRgb(c.R, c.G, c.B));
        }
    }

    private void OnStrokeChanged(object sender, SelectionChangedEventArgs e)
    {
        // The ComboBox's initial selection fires this during InitializeComponent,
        // before the Canvas field is assigned — ignore until the canvas exists.
        if (Canvas is null)
        {
            return;
        }

        if (StrokeCombo.SelectedItem is ComboBoxItem { Content: string text }
            && double.TryParse(text, out double width))
        {
            Canvas.ApplyStrokeToSelection(width);
        }
    }

    private void OnUndo(object sender, RoutedEventArgs e) => Canvas.Undo.Undo();

    private void OnRedo(object sender, RoutedEventArgs e) => Canvas.Undo.Redo();

    private void OnDelete(object sender, RoutedEventArgs e) => Canvas.DeleteSelected();

    private void OnCopy(object sender, RoutedEventArgs e)
    {
        try
        {
            System.Windows.Clipboard.SetImage(Canvas.RenderFlattened());
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(this, $"Copy failed: {ex.Message}", "SGrab");
        }
    }

    private void OnSave(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.SaveFileDialog
        {
            Filter = "PNG image (*.png)|*.png|JPEG image (*.jpg)|*.jpg",
            DefaultExt = ".png",
            FileName = $"SGrab-{DateTime.Now:yyyyMMdd-HHmmss}.png",
            InitialDirectory = _lastDirectory ?? string.Empty,
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        try
        {
            SaveFlattened(dialog.FileName);
            _lastDirectory = Path.GetDirectoryName(dialog.FileName);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(this, $"Save failed: {ex.Message}", "SGrab");
        }
    }

    private void SaveFlattened(string path)
    {
        var bitmap = Canvas.RenderFlattened();
        string ext = Path.GetExtension(path).ToLowerInvariant();
        BitmapEncoder encoder = ext is ".jpg" or ".jpeg"
            ? new JpegBitmapEncoder { QualityLevel = 92 }
            : new PngBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bitmap));

        using var stream = File.Create(path);
        encoder.Save(stream);
    }

    private void OnTextEditRequested(object? sender, TextAnnotation annotation)
    {
        _editing = annotation;
        _editBefore = annotation.Clone();

        // Map the object's top-left (canvas coords) into window coords.
        Point windowPoint = PointFromScreen(Canvas.PointToScreen(new Point(annotation.X, annotation.Y)));
        EditBox.Margin = new Thickness(windowPoint.X, windowPoint.Y, 0, 0);
        EditBox.FontSize = annotation.FontSize;
        EditBox.Text = annotation.Text;
        EditBox.Visibility = Visibility.Visible;
        EditBox.Focus();
        EditBox.SelectAll();
    }

    private void OnEditBoxKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case System.Windows.Input.Key.Enter:
                CommitEdit();
                e.Handled = true;
                break;
            case System.Windows.Input.Key.Escape:
                CancelEdit();
                e.Handled = true;
                break;
        }
    }

    private void OnEditBoxLostFocus(object sender, RoutedEventArgs e) => CommitEdit();

    private void CommitEdit()
    {
        if (_editing is { } annotation && _editBefore is { } before)
        {
            annotation.Text = EditBox.Text;
            Canvas.NotifyObjectModified(annotation, before);
        }

        HideEdit();
    }

    private void CancelEdit()
    {
        if (_editing is { } annotation && _editBefore is { } before)
        {
            annotation.CopyFrom(before);
            Canvas.InvalidateVisual();
        }

        HideEdit();
    }

    private void HideEdit()
    {
        _editing = null;
        _editBefore = null;
        EditBox.Visibility = Visibility.Collapsed;
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        base.OnPreviewKeyDown(e);

        // Don't steal Ctrl+Z/Y while typing in the text box.
        if (EditBox.Visibility == Visibility.Visible)
        {
            return;
        }

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
            case System.Windows.Input.Key.S:
                OnSave(this, new RoutedEventArgs());
                e.Handled = true;
                break;
            case System.Windows.Input.Key.C:
                OnCopy(this, new RoutedEventArgs());
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
