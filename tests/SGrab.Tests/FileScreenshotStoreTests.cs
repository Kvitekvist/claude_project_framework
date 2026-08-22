using System.Drawing;
using SGrab.Models;
using SGrab.Services;
using Xunit;

namespace SGrab.Tests;

public class FileScreenshotStoreTests
{
    [Fact]
    public void Save_WritesFilesAndManifestAndAddsItem()
    {
        string root = NewTempRoot();
        var store = new FileScreenshotStore(root);

        using var image = NewImage(10, 8);
        Screenshot shot = store.Save(image);

        Assert.Single(store.Items);
        Assert.Equal(10, shot.Width);
        Assert.Equal(8, shot.Height);
        Assert.True(File.Exists(shot.ImagePath));
        Assert.True(File.Exists(shot.ThumbPath));
        Assert.True(File.Exists(Path.Combine(root, "index.json")));
    }

    [Fact]
    public void Reload_RestoresItemsNewestFirst()
    {
        string root = NewTempRoot();

        var first = new FileScreenshotStore(root);
        using (var a = NewImage())
        {
            first.Save(a);
        }

        Thread.Sleep(5); // ensure a distinct CreatedUtc
        Screenshot newer;
        using (var b = NewImage())
        {
            newer = first.Save(b);
        }

        var reloaded = new FileScreenshotStore(root);

        Assert.Equal(2, reloaded.Items.Count);
        Assert.Equal(newer.Id, reloaded.Items[0].Id);
    }

    [Fact]
    public void Delete_RemovesItemAndFiles()
    {
        string root = NewTempRoot();
        var store = new FileScreenshotStore(root);

        Screenshot shot;
        using (var image = NewImage())
        {
            shot = store.Save(image);
        }

        store.Delete(shot.Id);

        Assert.Empty(store.Items);
        Assert.False(File.Exists(shot.ImagePath));
        Assert.False(File.Exists(shot.ThumbPath));
    }

    [Fact]
    public void Save_RaisesChanged()
    {
        string root = NewTempRoot();
        var store = new FileScreenshotStore(root);
        int raised = 0;
        store.Changed += (_, _) => raised++;

        using (var image = NewImage())
        {
            store.Save(image);
        }

        Assert.Equal(1, raised);
    }

    private static CapturedImage NewImage(int width = 12, int height = 9)
    {
        var bitmap = new Bitmap(width, height);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.Clear(Color.CornflowerBlue);
        }

        return new CapturedImage(bitmap);
    }

    private static string NewTempRoot()
        => Path.Combine(Path.GetTempPath(), "SGrabTests", Guid.NewGuid().ToString("N"));
}
