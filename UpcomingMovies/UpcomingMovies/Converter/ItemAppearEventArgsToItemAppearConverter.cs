using System;
using System.Globalization;
using Xamarin.Forms;

namespace UpcomingMovies.Converter
{
    public class ItemAppearEventArgsToItemAppearConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as ItemVisibilityEventArgs;
            return eventArgs.Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
