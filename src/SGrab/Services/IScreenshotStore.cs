using SGrab.Models;

namespace SGrab.Services;

/// <summary>
/// Persists captures to the local library and exposes them newest-first.
/// Raises <see cref="Changed"/> whenever the collection is modified so UI
/// (the filmstrip, TICKET-0013) can refresh.
/// </summary>
public interface IScreenshotStore
{
    /// <summary>Raised on the calling thread after any add or delete.</summary>
    event EventHandler? Changed;

    /// <summary>All saved screenshots, newest first.</summary>
    IReadOnlyList<Screenshot> Items { get; }

    /// <summary>Saves a capture (PNG + thumbnail + manifest entry) and returns its metadata.</summary>
    Screenshot Save(CapturedImage image);

    /// <summary>Deletes a screenshot's files and manifest entry. No-op if unknown.</summary>
    void Delete(string id);
}
