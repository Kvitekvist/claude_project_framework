using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Point = System.Windows.Point;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace SGrab.Views;

/// <summary>
/// Full-screen dimmed overlay for rubber-band region selection. Returns the
/// chosen rectangle in absolute physical screen pixels, or null if cancelled.
///
/// Selection is tracked in the overlay's DIP space and converted to physical
/// pixels via the window's DPI scale. This is exact on single-monitor and
/// uniform-DPI multi-monitor setups; on mixed-DPI setups the selection stays
/// WYSIWYG on the overlay's own monitor (see TICKET-0008 notes).
/// </summary>
public partial class CaptureOverlayWindow : Window
{
    private const double MinSelectionDip = 4.0;

    private Point _start;
    private bool _dragging;
    private TaskCompletionSource<Int32Rect?>? _completion;

    public CaptureOverlayWindow()
    {
        InitializeComponent();

        // Cover the entire virtual desktop (all monitors).
        Left = SystemParameters.VirtualScreenLeft;
        Top = SystemParameters.VirtualScreenTop;
        Width = SystemParameters.VirtualScreenWidth;
        Height = SystemParameters.VirtualScreenHeight;
    }

    /// <summary>Shows the overlay and completes with the selected physical rectangle.</summary>
    public Task<Int32Rect?> SelectRegionAsync()
    {
        _completion = new TaskCompletionSource<Int32Rect?>();
        Show();
        Activate();
        Focus();
        return _completion.Task;
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        _start = e.GetPosition(RootCanvas);
        _dragging = true;
        Hint.Visibility = Visibility.Collapsed;
        CaptureMouse();
        UpdateSelectionVisual(_start, _start);
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        if (_dragging)
        {
            UpdateSelectionVisual(_start, e.GetPosition(RootCanvas));
        }
    }

    protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonUp(e);
        if (!_dragging)
        {
            return;
        }

        _dragging = false;
        ReleaseMouseCapture();
        Complete(new Rect(_start, e.GetPosition(RootCanvas)));
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if (e.Key == Key.Escape)
        {
            Cancel();
        }
    }

    private void UpdateSelectionVisual(Point a, Point b)
    {
        var rect = new Rect(a, b);
        Canvas.SetLeft(SelectionRect, rect.X);
        Canvas.SetTop(SelectionRect, rect.Y);
        SelectionRect.Width = rect.Width;
        SelectionRect.Height = rect.Height;
        SelectionRect.Visibility = Visibility.Visible;

        var dpi = VisualTreeHelper.GetDpi(this);
        int pw = (int)Math.Round(rect.Width * dpi.DpiScaleX);
        int ph = (int)Math.Round(rect.Height * dpi.DpiScaleY);
        SizeText.Text = $"{pw} × {ph}";
        Canvas.SetLeft(SizeBadge, rect.X);
        Canvas.SetTop(SizeBadge, Math.Max(0, rect.Y - 24));
        SizeBadge.Visibility = Visibility.Visible;
    }

    private void Complete(Rect selectionDip)
    {
        if (selectionDip.Width < MinSelectionDip || selectionDip.Height < MinSelectionDip)
        {
            Cancel();
            return;
        }

        var dpi = VisualTreeHelper.GetDpi(this);

        // Absolute physical screen pixels: (window origin + selection) * DPI scale.
        int x = (int)Math.Round((Left + selectionDip.X) * dpi.DpiScaleX);
        int y = (int)Math.Round((Top + selectionDip.Y) * dpi.DpiScaleY);
        int w = (int)Math.Round(selectionDip.Width * dpi.DpiScaleX);
        int h = (int)Math.Round(selectionDip.Height * dpi.DpiScaleY);

        var result = new Int32Rect(x, y, w, h);

        // Hide before completing so the capture doesn't include the dim overlay.
        Hide();
        _completion?.TrySetResult(result);
        Close();
    }

    private void Cancel()
    {
        Hide();
        _completion?.TrySetResult(null);
        Close();
    }
}
