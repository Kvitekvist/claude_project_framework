using System.Windows;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;

namespace SGrab.Models.Annotations;

/// <summary>
/// Base class for a retained-mode annotation drawn over the capture. Every
/// object has an axis-aligned bounding box plus a stroke colour and width; it
/// knows how to render itself into a <see cref="DrawingContext"/> and how to
/// clone/copy itself for undo snapshots.
/// </summary>
public abstract class AnnotationObject
{
    public double X { get; set; }

    public double Y { get; set; }

    public double Width { get; set; }

    public double Height { get; set; }

    public Color Color { get; set; } = Colors.Red;

    public double StrokeWidth { get; set; } = 3;

    public Rect Bounds => new(X, Y, Width, Height);

    /// <summary>Hit-test in canvas coordinates. Default is the bounding box.</summary>
    public virtual bool HitTest(Point point) => Bounds.Contains(point);

    public abstract void Draw(DrawingContext dc, double pixelsPerDip);

    public abstract AnnotationObject Clone();

    /// <summary>Copies all mutable state from <paramref name="other"/> (same type expected).</summary>
    public virtual void CopyFrom(AnnotationObject other)
    {
        X = other.X;
        Y = other.Y;
        Width = other.Width;
        Height = other.Height;
        Color = other.Color;
        StrokeWidth = other.StrokeWidth;
    }

    /// <summary>Flips negative width/height so <see cref="Bounds"/> is well-formed.</summary>
    public void Normalize()
    {
        if (Width < 0)
        {
            X += Width;
            Width = -Width;
        }

        if (Height < 0)
        {
            Y += Height;
            Height = -Height;
        }
    }

    protected Pen CreatePen()
    {
        var pen = new Pen(new SolidColorBrush(Color), StrokeWidth);
        pen.Freeze();
        return pen;
    }
}
