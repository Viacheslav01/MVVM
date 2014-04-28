using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MVVM_Article.Converters
{
	public class BoolToVisibilityConverter
		: IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (value as bool?) == true
				? Visibility.Visible
				: Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
