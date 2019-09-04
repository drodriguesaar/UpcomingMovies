using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace UpcomingMovies.Converter
{
    public class VoteFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var length = ((string)value).Length;

            if (length <= 3)
            {
                return Device.GetNamedSize(NamedSize.Header, typeof(Label));
            }
            else
            {
                return Device.GetNamedSize(NamedSize.Title, typeof(Label));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
