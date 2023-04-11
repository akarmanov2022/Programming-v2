using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Contacts.ViewModel;

/// <summary>
/// Конвертирует значение <see cref="string"/> в <see cref="Visibility"/>.
/// </summary>
public class StringToVisibilityConverter : IValueConverter
{
    /// <inheritdoc/>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Enum.Parse(typeof(Visibility), value.ToString() ?? string.Empty);
    }

    /// <inheritdoc/>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return ((Visibility)value).ToString();
    }
}