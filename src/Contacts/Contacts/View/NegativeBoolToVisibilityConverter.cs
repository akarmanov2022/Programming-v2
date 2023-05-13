using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Contacts.View;

/// <summary>
/// Конвертирует значение <see cref="bool"/> в <see cref="Visibility"/>.
/// </summary>
public class NegativeBoolToVisibilityConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !bool.Parse(value.ToString() ?? bool.FalseString) ? Visibility.Visible : Visibility.Hidden;
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !Visibility.Visible.Equals(value);
    }
}