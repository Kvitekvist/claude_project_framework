namespace SGrab.Models;

/// <summary>
/// Metadata for one saved screenshot in the library. The pixel data lives on
/// disk at <see cref="ImagePath"/> (full PNG) and <see cref="ThumbPath"/>
/// (thumbnail); this record is what the manifest stores and the UI binds to.
/// </summary>
public sealed class Screenshot
{
    public string Id { get; init; } = string.Empty;

    public DateTime CreatedUtc { get; init; }

    public int Width { get; init; }

    public int Height { get; init; }

    public string ImagePath { get; init; } = string.Empty;

    public string ThumbPath { get; init; } = string.Empty;
}
