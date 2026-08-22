using System.Globalization;
using System.Windows;
using System.Windows.Media;
using FlowDirection = System.Windows.FlowDirection;
using FontFamily = System.Windows.Media.FontFamily;
using Point = System.Windows.Point;

namespace SGrab.Models.Annotations;

/// <summary>A text label. Its bounds track the rendered text extent for hit-testing.</summary>
public sealed class TextAnnotation : AnnotationObject
{
    private static readonly Typeface Typeface = new(
        new FontFamily("Segoe UI"), FontStyles.Normal, FontWeights.SemiBold, FontStretches.Normal);

    public string Text { get; set; } = "Text";

    public double FontSize { get; set; } = 24;

    public override void Draw(DrawingContext dc, double pixelsPerDip)
    {
        var brush = new SolidColorBrush(Color);
        brush.Freeze();

        var formatted = new FormattedText(
            string.IsNullOrEmpty(Text) ? " " : Text,
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            Typeface,
            FontSize,
            brush,
            pixelsPerDip);

        dc.DrawText(formatted, new Point(X, Y));

        // Keep the bounding box in sync so selection/hit-testing match the glyphs.
        Width = formatted.WidthIncludingTrailingWhitespace;
        Height = formatted.Height;
    }

    public override void CopyFrom(AnnotationObject other)
    {
        base.CopyFrom(other);
        if (other is TextAnnotation text)
        {
            Text = text.Text;
            FontSize = text.FontSize;
        }
    }

    public override AnnotationObject Clone()
    {
        var clone = new TextAnnotation();
        clone.CopyFrom(this);
        return clone;
    }
}
