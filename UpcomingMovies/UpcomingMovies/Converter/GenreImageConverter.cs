using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace UpcomingMovies.Converter
{
    public class GenreImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var genre = ((string)value).ToLower();

            switch (genre)
            {
                case "war":
                case "western":
                case "action":
                    return "ic_actionmovie.png";

                case "comedy":
                    return "ic_comedymovie.png";

                case "adventure":
                    return "ic_adventuremovie.png";

                case "animation":
                case "family":
                    return "ic_animationmovie.png";

                case "drama":
                case "romance":
                    return "ic_dramamovie.png";

                case "documentary":
                case "music":
                    return "ic_documentarymovie.png";

                case "history":
                    return "ic_historymovie.png";

                case "crime":
                case "horror":
                case "thriller":
                case "mystery":
                    return "ic_crimemovie.png";

                case "science fiction":
                    return "ic_fantasymovie.png";

                default:
                    return "ic_postermovie.png";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
