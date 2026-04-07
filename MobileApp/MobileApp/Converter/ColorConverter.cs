using System;
using System.Globalization;
using Xamarin.Forms;

namespace MobileApp.Converter
{
    public class ColorConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var date = (DateTime)value;
            if (TimeSpan.FromDays(3) < (date - DateTime.Now) && TimeSpan.FromDays(6) > (date - DateTime.Now))
                return Color.PeachPuff;
            else if (TimeSpan.FromDays(3) >= (date - DateTime.Now))
                return Color.LightCoral;
            else
                return Color.Transparent;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
