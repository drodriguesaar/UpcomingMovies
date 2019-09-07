using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace UpcomingMovies.Converter
{
    class ScoreColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return Color.Default;
            }

            int score;

            var scorestring = ((string)value);

            if (scorestring.Contains("."))
            {
                scorestring = scorestring.Split('.')[0];
            }

            int.TryParse(scorestring, out score);

            switch (Decimal.Truncate(score))
            {
                case 0:
                case 1:
                case 2:
                    return Color.Red;
                case 3:
                case 4:
                    return Color.OrangeRed;
                case 5:
                case 6:
                    return Color.FromHex("#dbaf2e");
                case 7:
                case 8:
                    return Color.YellowGreen;
                case 9:
                case 10:
                    return Color.Green;
                default:
                    return Color.Default;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
