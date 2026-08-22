using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace SGrab.Common;

/// <summary>Conversions between GDI bitmaps / files and WPF <see cref="BitmapSource"/>.</summary>
public static class ImageInterop
{
    /// <summary>Converts a GDI bitmap to a frozen WPF <see cref="BitmapSource"/>.</summary>
    public static BitmapSource ToBitmapSource(Bitmap bitmap)
    {
        using var stream = new MemoryStream();
        bitmap.Save(stream, ImageFormat.Png);
        stream.Position = 0;
        return Decode(stream);
    }

    /// <summary>Loads an image file into a frozen <see cref="BitmapSource"/> without locking the file.</summary>
    public static BitmapSource LoadFromFile(string path)
    {
        using var stream = File.OpenRead(path);
        return Decode(stream);
    }

    private static BitmapSource Decode(Stream stream)
    {
        var image = new BitmapImage();
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = stream;
        image.EndInit();
        image.Freeze();
        return image;
    }
}
