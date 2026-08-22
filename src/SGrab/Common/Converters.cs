using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace SGrab.Common;

/// <summary>Loads a file path into a frozen image source for binding to Image.Source.</summary>
public sealed class PathToImageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string path && File.Exists(path) ? ImageInterop.LoadFromFile(path) : null;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// <summary>Maps false → Visible, true → Collapsed (for empty-state placeholders).</summary>
public sealed class InverseBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is true ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
