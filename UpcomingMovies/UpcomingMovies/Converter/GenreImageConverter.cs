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
                case "western":
                    return "ic_westernmovie.png";

                case "war":
                    return "ic_warmovie.png";

                case "action":
                    return "ic_actionmovie.png";

                case "comedy":
                    return "ic_comedymovie.png";

                case "adventure":
                    return "ic_adventuremovie.png";

                case "family":
                    return "ic_familymovie.png";

                case "animation":
                    return "ic_animationmovie.png";

                case "romance":
                    return "ic_romancemovie.png";

                case "drama":
                    return "ic_dramamovie.png";

                case "music":
                    return "ic_musicalmovie.png";

                case "documentary":
                    return "ic_documentarymovie.png";

                case "history":
                    return "ic_historymovie.png";

                case "thriller":
                    return "ic_thrillermovie.png";

                case "horror":
                    return "ic_horrormovie.png";

                case "mystery":
                    return "ic_mysterymovie.png";

                case "crime":
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
