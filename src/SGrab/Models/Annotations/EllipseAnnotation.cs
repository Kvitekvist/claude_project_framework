using System.Windows.Media;
using Point = System.Windows.Point;

namespace SGrab.Models.Annotations;

public sealed class EllipseAnnotation : AnnotationObject
{
    public override void Draw(DrawingContext dc, double pixelsPerDip)
    {
        var bounds = Bounds;
        var center = new Point(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
        dc.DrawEllipse(null, CreatePen(), center, bounds.Width / 2, bounds.Height / 2);
    }

    public override bool HitTest(Point point)
    {
        var bounds = Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0)
        {
            return false;
        }

        double nx = (point.X - (bounds.X + bounds.Width / 2)) / (bounds.Width / 2);
        double ny = (point.Y - (bounds.Y + bounds.Height / 2)) / (bounds.Height / 2);
        return (nx * nx) + (ny * ny) <= 1.0;
    }

    public override AnnotationObject Clone()
    {
        var clone = new EllipseAnnotation();
        clone.CopyFrom(this);
        return clone;
    }
}
