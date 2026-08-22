using System.Windows.Media;

namespace SGrab.Models.Annotations;

public sealed class RectangleAnnotation : AnnotationObject
{
    public override void Draw(DrawingContext dc, double pixelsPerDip)
        => dc.DrawRectangle(null, CreatePen(), Bounds);

    public override AnnotationObject Clone()
    {
        var clone = new RectangleAnnotation();
        clone.CopyFrom(this);
        return clone;
    }
}
