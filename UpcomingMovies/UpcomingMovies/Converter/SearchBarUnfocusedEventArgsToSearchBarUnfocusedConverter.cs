using System;
using System.Globalization;
using Xamarin.Forms;

namespace UpcomingMovies.Converter
{
    public class SearchBarUnfocusedEventArgsToSearchBarUnfocusedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as FocusEventArgs;
            return eventArgs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
