using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;
using UpcomingMovies.Component;
using UpcomingMovies.Consts;
using UpcomingMovies.Infra;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    class SearchResultViewModel : ViewModelBase
    {
        readonly MovieService _movieService;
        readonly MovieParameter _movieParameter;
        readonly INavigation _navigation;

        bool _IsVisible;
        string _SearchText;
        bool _IsRefreshing;
        ObservableCollection<MovieModel> _Movies;

        public SearchResultViewModel()
        {

        }

        public SearchResultViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _movieParameter = new MovieParameter();
            _movieService = new MovieService();
            Movies = new ObservableCollection<MovieModel>();
            GetMovieCommand = new Command<MovieModel>(GetMovie);
            MovieAppearCommand = new Command<MovieModel>(MovieAppear);
        }

        public ICommand GetMovieCommand { get; private set; }
        public ICommand MovieAppearCommand { get; private set; }

        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                if (_IsVisible != value)
                {
                    _IsVisible = value;
                    OnPropertyChanged("IsVisible");
                }
            }
        }
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                if(_SearchText != value)
                {
                    _SearchText = value;
                    OnPropertyChanged("SearchText");
                }
            }
        }
        public bool IsRefreshing
        {
            get { return _IsRefreshing; }
            set
            {
                if (_IsRefreshing != value)
                {
                    _IsRefreshing = value;
                    OnPropertyChanged("IsRefreshing");
                }
            }
        }
        public bool NavigatedToDetails { get; set; }
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


        public void SearchByText(string searchText)
        {
            this.SearchText = string.Format("Results to {0}", searchText);
            this.IsVisible = false;

            Global.Instance.Toast.ShortToast(string.Format("Searching by {0}...", searchText));

            _movieParameter.Page = 1;
            _movieParameter.Query = HttpUtility.UrlEncode(searchText);
            _movieParameter.Resource = MoviesApiResourcesConsts.SEARCH_MOVIE;
            _movieService.GetMovies(_movieParameter).ContinueWith((moviesList) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (moviesList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var movies = moviesList.Result;
                        if (!movies.Any())
                        {
                            Global.Instance.Toast.ShortToast("No movies found...");
                        }
                        else
                        {
                            Movies.Clear();
                            PopulateListView(movies);
                        }
                        this.IsVisible = true;
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
        void GetMovie(MovieModel movie)
        {
            NavigatedToDetails = true;
            _navigation.PushAsync(new MoviePage { MovieID = movie.Id });
        }

        void MovieAppear(MovieModel movie)
        {
            if (!movie.Position.Equals(Movies.Count - 2))
            {
                return;
            }

            var resource = MoviesApiResourcesConsts.SEARCH_MOVIE;
            var searchQuery = HttpUtility.UrlEncode(SearchText);
#if __ANDROID__
            App.Toast.ShortToast("Loading...");
#endif
            _movieParameter.Page = _movieParameter.Page + 1;
            _movieParameter.Resource = resource;
            _movieService.GetMovies(_movieParameter).ContinueWith((moviesList) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (moviesList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var movies = moviesList.Result;
                        if (!movies.Any())
                        {
                            Global.Instance.Toast.ShortToast("No more movies...");
                            return;
                        }
                        PopulateListView(movies);
                    }
                });
            });
        }
    }
}
