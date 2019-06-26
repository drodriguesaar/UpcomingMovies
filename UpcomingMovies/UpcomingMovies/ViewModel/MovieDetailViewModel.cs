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
            _movieParameter.Id = id;
            _movieComponent.GetMovie(_movieParameter).ContinueWith((movie) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (movie.Status == TaskStatus.RanToCompletion)
                    {
                        Movie = movie.Result;
                        IsReady = true;
                    }
                });
            });
        }
    }
}
