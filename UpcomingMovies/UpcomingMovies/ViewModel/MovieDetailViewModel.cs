using System.ComponentModel;
using System.Threading.Tasks;
using UpcomingMovies.Component;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class MovieDetailViewModel : ViewModelBase
    {
        MovieService _movieComponent;
        MovieParameter _movieParameter;
        MovieModel _Movie;
        Color _Color;
        public Color Color
        {
            get { return _Color; }
            set
            {
                if (_Color != value)
                {
                    _Color = value;
                    OnPropertyChanged("Color");
                }
            }
        }

        bool _IsReady;

        public MovieModel Movie
        {
            get { return _Movie; }
            set
            {
                if (_Movie != value)
                {
                    _Movie = value;
                    OnPropertyChanged("Movie");
                }
            }
        }
        public bool IsReady
        {
            get { return _IsReady; }
            set
            {
                if (_IsReady != value)
                {
                    _IsReady = value;
                    OnPropertyChanged("IsReady");
                }
            }
        }

        public MovieDetailViewModel()
        {
            _movieParameter = new MovieParameter();
            _movieComponent = new MovieService();
            
        }

        public void GetMovieDetails(int id)
        {
            IsReady = false;
            Color = Color.Silver;
            _movieParameter.Id = id;
            Device.BeginInvokeOnMainThread(() =>
            {
                _movieComponent.GetMovie(_movieParameter).ContinueWith((movie) =>
                    {
                        if (movie.Status == TaskStatus.RanToCompletion)
                        {
                            Movie = movie.Result;
                            IsReady = true;
                            Color = Color.Transparent;
                        }
                    });
            });
        }
    }
}
