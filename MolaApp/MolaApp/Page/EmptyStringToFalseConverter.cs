using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MolaApp.Page
{
    public class EmptyStringToFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException("This conversion is not possible");
        }
    }
}
