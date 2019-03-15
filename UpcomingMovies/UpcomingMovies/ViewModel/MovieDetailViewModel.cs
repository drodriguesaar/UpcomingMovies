using UpcomingMovies.Component;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using System.ComponentModel;
using System.Threading.Tasks;

namespace UpcomingMovies.ViewModel
{
    public class MovieDetailViewModel : INotifyPropertyChanged
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
        public event PropertyChangedEventHandler PropertyChanged;
        public MovieDetailViewModel()
        {
            _movieParameter = new MovieParameter();
            _movieComponent = new MovieService();
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async Task GetMovieDetails(int id)
        {
            _movieParameter.Id = id;
            Movie = await _movieComponent.GetMovie(_movieParameter);
            IsReady = true;
        }
    }
}
