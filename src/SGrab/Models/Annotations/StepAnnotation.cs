using System.Globalization;
using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;
using FlowDirection = System.Windows.FlowDirection;
using FontFamily = System.Windows.Media.FontFamily;
using Pen = System.Windows.Media.Pen;
using Point = System.Windows.Point;

namespace SGrab.Models.Annotations;

/// <summary>A filled, numbered step bubble (1, 2, 3…) with a white ring and digit.</summary>
public sealed class StepAnnotation : AnnotationObject
{
    private static readonly Typeface Typeface = new(
        new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);

    public StepAnnotation()
    {
        Width = 34;
        Height = 34;
    }

    public int Number { get; set; } = 1;

    public override void Draw(DrawingContext dc, double pixelsPerDip)
    {
        var bounds = Bounds;
        var center = new Point(bounds.X + (bounds.Width / 2), bounds.Y + (bounds.Height / 2));
        double radius = Math.Min(bounds.Width, bounds.Height) / 2;

        var fill = new SolidColorBrush(Color);
        fill.Freeze();
        var ring = new Pen(Brushes.White, Math.Max(1.5, StrokeWidth * 0.6));
        ring.Freeze();
        dc.DrawEllipse(fill, ring, center, radius, radius);

        double fontSize = Math.Max(8, Math.Min(bounds.Width, bounds.Height) * 0.6);
        var text = new FormattedText(
            Number.ToString(CultureInfo.CurrentCulture),
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            Typeface,
            fontSize,
            Brushes.White,
            pixelsPerDip);
        dc.DrawText(text, new Point(center.X - (text.Width / 2), center.Y - (text.Height / 2)));
    }

    public override void CopyFrom(AnnotationObject other)
    {
        base.CopyFrom(other);
        if (other is StepAnnotation step)
        {
            Number = step.Number;
        }
    }

    public override AnnotationObject Clone()
    {
        var clone = new StepAnnotation();
        clone.CopyFrom(this);
        return clone;
    }
}
