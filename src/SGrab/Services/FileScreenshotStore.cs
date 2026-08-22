using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using SGrab.Models;

namespace SGrab.Services;

/// <summary>
/// File-backed screenshot library. Layout under the library root:
/// <code>
///   images/{id}.png   full-resolution capture
///   thumbs/{id}.png   thumbnail (max 200px on the long edge)
///   index.json        manifest of <see cref="Screenshot"/> metadata
/// </code>
/// The manifest is the source of truth for ordering; entries whose image file
/// has gone missing are dropped on load.
/// </summary>
public sealed class FileScreenshotStore : IScreenshotStore
{
    private const int ThumbnailMaxEdge = 200;

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    private readonly string _root;
    private readonly string _imagesDir;
    private readonly string _thumbsDir;
    private readonly string _manifestPath;
    private readonly List<Screenshot> _items = new();

    public FileScreenshotStore()
        : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SGrab", "Library"))
    {
    }

    public FileScreenshotStore(string root)
    {
        _root = root;
        _imagesDir = Path.Combine(_root, "images");
        _thumbsDir = Path.Combine(_root, "thumbs");
        _manifestPath = Path.Combine(_root, "index.json");

        Directory.CreateDirectory(_imagesDir);
        Directory.CreateDirectory(_thumbsDir);
        Load();
    }

    public event EventHandler? Changed;

    public IReadOnlyList<Screenshot> Items => _items;

    public Screenshot Save(CapturedImage image)
    {
        ArgumentNullException.ThrowIfNull(image);

        var createdUtc = DateTime.UtcNow;
        string id = $"{createdUtc:yyyyMMdd-HHmmssfff}-{Guid.NewGuid():N}"[..23];
        string imagePath = Path.Combine(_imagesDir, id + ".png");
        string thumbPath = Path.Combine(_thumbsDir, id + ".png");

        image.Bitmap.Save(imagePath, ImageFormat.Png);
        using (var thumb = CreateThumbnail(image.Bitmap, ThumbnailMaxEdge))
        {
            thumb.Save(thumbPath, ImageFormat.Png);
        }

        var shot = new Screenshot
        {
            Id = id,
            CreatedUtc = createdUtc,
            Width = image.Width,
            Height = image.Height,
            ImagePath = imagePath,
            ThumbPath = thumbPath,
        };

        _items.Insert(0, shot);
        SaveManifest();
        Changed?.Invoke(this, EventArgs.Empty);
        return shot;
    }

    public void Delete(string id)
    {
        int index = _items.FindIndex(s => s.Id == id);
        if (index < 0)
        {
            return;
        }

        var shot = _items[index];
        _items.RemoveAt(index);
        TryDeleteFile(shot.ImagePath);
        TryDeleteFile(shot.ThumbPath);
        SaveManifest();
        Changed?.Invoke(this, EventArgs.Empty);
    }

    private void Load()
    {
        if (!File.Exists(_manifestPath))
        {
            return;
        }

        try
        {
            var loaded = JsonSerializer.Deserialize<List<Screenshot>>(
                File.ReadAllText(_manifestPath), JsonOptions);
            if (loaded is null)
            {
                return;
            }

            _items.AddRange(loaded
                .Where(s => !string.IsNullOrEmpty(s.ImagePath) && File.Exists(s.ImagePath))
                .OrderByDescending(s => s.CreatedUtc));
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            // Corrupt or unreadable manifest: start empty rather than crash.
        }
    }

    private void SaveManifest()
    {
        var json = JsonSerializer.Serialize(_items, JsonOptions);
        File.WriteAllText(_manifestPath, json);
    }

    private static Bitmap CreateThumbnail(Bitmap source, int maxEdge)
    {
        double scale = Math.Min((double)maxEdge / source.Width, (double)maxEdge / source.Height);
        scale = Math.Min(scale, 1.0);
        int width = Math.Max(1, (int)Math.Round(source.Width * scale));
        int height = Math.Max(1, (int)Math.Round(source.Height * scale));

        var thumb = new Bitmap(width, height);
        using var graphics = Graphics.FromImage(thumb);
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.DrawImage(source, 0, 0, width, height);
        return thumb;
    }

    private static void TryDeleteFile(string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (IOException)
        {
            // Best-effort cleanup; the manifest entry is already gone.
        }
    }
}
