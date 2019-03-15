using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Windows.Input;
using UpcomingMovies.Component;
using UpcomingMovies.Consts;
using UpcomingMovies.Dependency;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class UpcomingMoviesViewModel : INotifyPropertyChanged
    {
        readonly MovieService _movieService;
        readonly MovieParameter _movieParameter;
        readonly IToast toast;
        readonly INavigation _navigation;

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

        public ObservableCollection<MovieModel> Movies
        {
            get { return _Movies; }
            set
            {
                if (_Movies != value)
                {
                    _Movies = value;
                    OnPropertyChanged("Movies");
                }
            }
        }

        public UpcomingMoviesViewModel()
        {

        }
        public UpcomingMoviesViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _movieService = new MovieService();
            _movieParameter = new MovieParameter();
            toast = DependencyService.Get<IToast>();
            Movies = new ObservableCollection<MovieModel>();

            GetMovieCommand = new Command<MovieModel>(GetMovie);
            SearchMovieCommand = new Command(SearchMovie);
            MovieAppearCommand = new Command<MovieModel>(MovieAppear);
            SearchMovieUnfocusedCommand = new Command(SearchUnfocused);
        }

        string _SearchText;
        bool _IsReady;
        bool _IsVisible;
        ObservableCollection<MovieModel> _Movies;

        public ICommand GetMovieCommand { get; private set; }
        public ICommand SearchMovieCommand { get; private set; }
        public ICommand SearchMovieUnfocusedCommand { get; private set; }
        public ICommand MovieAppearCommand { get; private set; }


        public void GetUpComingMovies()
        {
            _movieService.Resource = string.IsNullOrEmpty(_movieParameter.Query) ? MoviesApiResourcesConsts.UPCOMING_MOVIES : MoviesApiResourcesConsts.SEARCH_MOVIE;
            _movieParameter.Page = 1;
            _movieService.GetMovies(_movieParameter).ContinueWith((moviesList) =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (moviesList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        {
                            var movies = moviesList.Result;
                            if (!movies.Any())
                            {
                                toast.ShortToast("No movies found...");
                                return;
                            }
                            PopulateListView(movies);
                            IsReady = true;
                            IsVisible = true;
                        }
                    });
                });
        }
        void GetMovie(MovieModel movie)
        {
            _navigation.PushAsync(new MoviePage { MovieID = movie.Id });
        }
        void SearchMovie()
        {
            _movieParameter.Page = 1;
            _movieParameter.Query = HttpUtility.UrlEncode(SearchText);
            _movieService.Resource = MoviesApiResourcesConsts.SEARCH_MOVIE;
            _movieService.GetMovies(_movieParameter).ContinueWith((moviesList) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (moviesList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var movies = moviesList.Result;
                        if (!movies.Any())
                        {
                            toast.ShortToast("No movies found...");
                            return;
                        }
                        Movies.Clear();
                        PopulateListView(movies);
                    }
                }); 
            });
        }
        void SearchUnfocused()
        {
            if(string.IsNullOrEmpty(this.SearchText))
            {
                this.Movies.Clear();
                _movieParameter.Query = string.Empty;
                GetUpComingMovies();
            }
        }
        void MovieAppear(MovieModel movie)
        {
            if (!movie.Position.Equals(Movies.Count - 5))
            {
                return;
            }

            var resource = string.IsNullOrEmpty(SearchText) ? MoviesApiResourcesConsts.UPCOMING_MOVIES : MoviesApiResourcesConsts.SEARCH_MOVIE;
            var searchQuery = string.IsNullOrEmpty(SearchText) ? string.Empty : HttpUtility.UrlEncode(SearchText);

            _movieParameter.Page = _movieParameter.Page + 1;
            _movieService.Resource = resource;
            _movieService.GetMovies(_movieParameter).ContinueWith((moviesList) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (moviesList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var movies = moviesList.Result;
                        if (!movies.Any())
                        {
                            toast.ShortToast("No more movies...");
                            return;
                        }
                        PopulateListView(movies);
                    }
                });
            });
        }

        void PopulateListView(List<MovieModel> movies)
        {
            var position = Movies.Count();
            movies.ForEach(movie =>
            {
                position = position + 1;
                movie.Position = position;
                Movies.Add(movie);
            });
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
