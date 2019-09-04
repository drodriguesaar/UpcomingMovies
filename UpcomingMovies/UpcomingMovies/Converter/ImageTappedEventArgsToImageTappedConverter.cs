using System;
using System.Globalization;
using Xamarin.Forms;

namespace UpcomingMovies.Converter
{
    public class ImageTappedEventArgsToImageTappedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as TappedEventArgs;
            return eventArgs.Parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
