using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
namespace BookSearch.Util
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)string.IsNullOrEmpty(value as string));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
