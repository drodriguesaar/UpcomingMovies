using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Windows.Input;
using UpcomingMovies.Component;
using UpcomingMovies.Consts;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class UpcomingMoviesViewModel : INotifyPropertyChanged
    {
        readonly MovieService _movieService;
        readonly MovieParameter _movieParameter;
        readonly INavigation _navigation;

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

        public UpcomingMoviesViewModel()
        {

        }
        public UpcomingMoviesViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _movieService = new MovieService();
            _movieParameter = new MovieParameter();
            Movies = new ObservableCollection<MovieModel>();

            GetMovieCommand = new Command<MovieModel>(GetMovie);
            SearchMovieCommand = new Command(SearchMovie);
            MovieAppearCommand = new Command<MovieModel>(MovieAppear);
            SearchMovieUnfocusedCommand = new Command(SearchUnfocused);
            PullToRefreshCommand = new Command(PullToRefresh);
        }

        string _SearchText;
        bool _IsReady;
        bool _IsVisible;
        bool _IsRefreshing;
        ObservableCollection<MovieModel> _Movies;

        public ICommand GetMovieCommand { get; private set; }
        public ICommand SearchMovieCommand { get; private set; }
        public ICommand SearchMovieUnfocusedCommand { get; private set; }
        public ICommand MovieAppearCommand { get; private set; }
        public ICommand PullToRefreshCommand { get; private set; }

        void GetUpComingMovies()
        {
            _movieParameter.Resource = string.IsNullOrEmpty(_movieParameter.Query) ? MoviesApiResourcesConsts.UPCOMING_MOVIES : MoviesApiResourcesConsts.SEARCH_MOVIE;
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
                                App.Toast.ShortToast("No movies found...");
                                return;
                            }
                            Movies.Clear();
                            PopulateListView(movies);
                            IsReady = true;
                            IsVisible = true;
                            IsRefreshing = false;
                            SetMovieCacheTimeSpanData();
                        }
                    });
                });
        }
        void GetMovie(MovieModel movie)
        {
            NavigatedToDetails = true;
            _navigation.PushAsync(new MoviePage { MovieID = movie.Id });
        }
        void SearchMovie()
        {
            App.Toast.ShortToast("Searching...");
            _movieParameter.Page = 1;
            _movieParameter.Query = HttpUtility.UrlEncode(SearchText);
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
                            App.Toast.ShortToast("No movies found...");
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
            if (string.IsNullOrEmpty(SearchText))
            {
                Movies.Clear();
                _movieParameter.Query = string.Empty;
                GetMovies();
            }
        }
        void MovieAppear(MovieModel movie)
        {
            if (!movie.Position.Equals(Movies.Count - 2))
            {
                return;
            }

            var resource = string.IsNullOrEmpty(SearchText) ? MoviesApiResourcesConsts.UPCOMING_MOVIES : MoviesApiResourcesConsts.SEARCH_MOVIE;
            var searchQuery = string.IsNullOrEmpty(SearchText) ? string.Empty : HttpUtility.UrlEncode(SearchText);
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
                            App.Toast.ShortToast("No more movies...");
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
        void PullToRefresh()
        {
#if __ANDROID__
            App.Toast.ShortToast("Refreshing...");
#endif

            IsRefreshing = true;
            _movieParameter.Resource = MoviesApiResourcesConsts.UPCOMING_MOVIES;
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
                            App.Toast.ShortToast("No movies found...");
                            IsReady = true;
                            IsVisible = true;
                            IsRefreshing = false;
                            return;
                        }
                        Movies.Clear();
                        PopulateListView(movies);
                        IsReady = true;
                        IsVisible = true;
                        IsRefreshing = false;
                        UpdateMovieCacheTimeSpanData();
                    }
                });
            });

        }
        public void GetMovies()
        {
            App.DataBase.GetMovieDataCacheTimeSpanAsync().ContinueWith((movieCachedDataResult) =>
            {
                if (movieCachedDataResult.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    if (this.NavigatedToDetails)
                    {
                        this.NavigatedToDetails = false;
                        return;
                    }

                    var actualDate = DateTime.Now;
                    var movieCacheDateTimeSpan = movieCachedDataResult.Result;

                    var isBetweenTimeSpan = movieCacheDateTimeSpan != null && actualDate > movieCacheDateTimeSpan.DateFrom && actualDate < movieCacheDateTimeSpan.DateTo;

                    if (isBetweenTimeSpan)
                    {
                        App.DataBase.GetMoviesAsync().ContinueWith((moviesCachedResult) =>
                        {
                            if (moviesCachedResult.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                            {
                                var moviesCached = moviesCachedResult.Result;
                                Movies.Clear();
                                PopulateListView(moviesCached);
                                IsReady = true;
                                IsVisible = true;
                                _movieParameter.Page = 1;
                                _movieParameter.Resource = MoviesApiResourcesConsts.UPCOMING_MOVIES;
                            }
                        });
                    }
                    else
                    {
                        GetUpComingMovies();
                    }
                }
            });
        }
        void SetMovieCacheTimeSpanData()
        {
            var movieCacheTimeSpan = DateTime.Now;
            App.DataBase.InsertMovieDataCacheTimeSpanAsync(movieCacheTimeSpan, movieCacheTimeSpan.AddDays(1)).ContinueWith((result) =>
            {
                if(result.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    App.DataBase.InsertMoviesAsync(Movies.ToList()).Wait();
                }
            });
        }
        void UpdateMovieCacheTimeSpanData()
        {
            App.DataBase.DeleteMoviesAsync().ContinueWith((resultDelete) =>
            {
                if(resultDelete.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    var movieCacheTimeSpan = DateTime.Now;
                    App.DataBase.UpdateMovieDataCacheTimeSpanAsync(movieCacheTimeSpan, movieCacheTimeSpan.AddDays(1)).ContinueWith((resultUpdate) =>
                    {
                        App.DataBase.InsertMoviesAsync(Movies.ToList()).Wait();
                    });
                }
            });
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
