using System.Drawing;
using System.Windows;
using SGrab.Models;
using SGrab.Views;

namespace SGrab.Services;

/// <summary>
/// Region-select screen capture: shows the selection overlay, then grabs the
/// chosen rectangle from the screen with GDI <c>CopyFromScreen</c>.
/// </summary>
public sealed class CaptureService : ICaptureService
{
    // Give the compositor a moment to repaint the desktop without the overlay
    // before grabbing pixels.
    private static readonly TimeSpan OverlaySettleDelay = TimeSpan.FromMilliseconds(80);

    private bool _isCapturing;

    public event EventHandler<CapturedImage>? CaptureCompleted;

    public async Task StartCaptureAsync()
    {
        if (_isCapturing)
        {
            return;
        }

        _isCapturing = true;
        try
        {
            var overlay = new CaptureOverlayWindow();
            Int32Rect? region = await overlay.SelectRegionAsync();
            if (region is not { Width: > 0, Height: > 0 } rect)
            {
                return;
            }

            await Task.Delay(OverlaySettleDelay);

            var bitmap = CaptureRegion(rect);
            CaptureCompleted?.Invoke(this, new CapturedImage(bitmap));
        }
        finally
        {
            _isCapturing = false;
        }
    }

    private static Bitmap CaptureRegion(Int32Rect rect)
    {
        var bitmap = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.CopyFromScreen(rect.X, rect.Y, 0, 0,
            new System.Drawing.Size(rect.Width, rect.Height), CopyPixelOperation.SourceCopy);
        return bitmap;
    }
}
