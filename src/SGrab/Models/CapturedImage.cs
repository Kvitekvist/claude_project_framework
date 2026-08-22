using System.Drawing;

namespace SGrab.Models;

/// <summary>
/// A screen capture result. Wraps the raw GDI bitmap; ownership transfers to
/// the receiver, which is responsible for disposing it (or the storage layer
/// once TICKET-0009 lands).
/// </summary>
public sealed class CapturedImage : IDisposable
{
    public CapturedImage(Bitmap bitmap) => Bitmap = bitmap;

    public Bitmap Bitmap { get; }

    public int Width => Bitmap.Width;

    public int Height => Bitmap.Height;

    public void Dispose() => Bitmap.Dispose();
}
