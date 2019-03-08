using UpcomingMovies.Component;
using UpcomingMovies.Consts;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Movies.ViewModel
{
    public class UpcomingMoviesViewModel : INotifyPropertyChanged
    {
        readonly MovieComponent _movieComponent;
        readonly MovieParameter _movieParameter;
        UpcomingMovieModel _UpcomingMovies;
        string _SearchText;
        bool _IsReady;
        bool _IsVisible;

        public bool IsReady
        {
            get { return _IsReady; }
            set
            {
                if (_IsReady != value)
                {
                    _IsReady = value; OnPropertyChanged("IsReady");
                }
            }
        }
        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                if (_IsVisible != value)
                {
                    _IsVisible = value; OnPropertyChanged("IsVisible");
                }
            }
        }
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                if (_SearchText != value)
                {
                    _SearchText = value;
                    OnPropertyChanged("SearchText");
                }
            }
        }
        public UpcomingMovieModel UpcomingMovies
        {
            get { return _UpcomingMovies; }
            set
            {
                if (_UpcomingMovies != value)
                {
                    _UpcomingMovies = value;
                    OnPropertyChanged("UpcomingMovies");
                }
            }
        }

        public ICommand MovieTappedCommand { get; private set; }
        public ICommand SearchMovieCommand { get; private set; }
        public ICommand MovieAppearingCommand { get; private set; }

        public UpcomingMoviesViewModel()
        {
            MovieTappedCommand = new Command(MovieTapped);
            SearchMovieCommand = new Command(SearchMovie);
            MovieAppearingCommand = new Command(MovieAppearing);
        }

        void MovieTapped()
        {

        }

        void SearchMovie()
        {

        }

        void MovieAppearing()
        {

        }

        async Task GetUpcomingMovies(int page)
        {
            _movieParameter.Page = page;
            _movieComponent.Resource = MoviesApiResourcesConsts.UPCOMING_MOVIES;
            UpcomingMovies = await _movieComponent.GetMovies(_movieParameter);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;


    }
}
