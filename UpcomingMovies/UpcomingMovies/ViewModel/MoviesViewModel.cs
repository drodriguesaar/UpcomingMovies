using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UpcomingMovies.Component;
using UpcomingMovies.Consts;
using UpcomingMovies.Infra;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class MoviesViewModel : ViewModelBase
    {
        readonly INavigation _navigation;
        readonly MovieService _movieService;
        readonly MovieParameter _movieParameter;

        bool _IsReady;
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

        bool _IsVisibleMovies;
        public bool IsVisibleMovies
        {
            get { return _IsVisibleMovies; }
            set
            {
                if (_IsVisibleMovies != value)
                {
                    _IsVisibleMovies = value;
                    OnPropertyChanged("IsVisibleMovies");
                }
            }
        }

        bool _IsRefreshing;
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

        string _SearchText;
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

        ObservableCollection<MovieModel> _Movies;
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

        public MoviesViewModel()
        {
        }
        public MoviesViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _movieService = new MovieService();
            _movieParameter = new MovieParameter();

            Movies = new ObservableCollection<MovieModel>();

            GetMovieCommand = new Command<MovieModel>(GetMovie);
            SearchMovieCommand = new Command(SearchMovie);
            MovieAppearCommand = new Command<MovieModel>(MovieAppear);
            PullToRefreshCommand = new Command(PullToRefresh);
        }


        public ICommand GetMovieCommand { get; set; }
        public ICommand SearchMovieCommand { get; set; }
        public ICommand MovieAppearCommand { get; set; }
        public ICommand PullToRefreshCommand { get; set; }

        public void Init()
        {
            if (_Navigated)
            {
                _Navigated = false;
                return;
            }

            this.IsVisibleMovies = false;
            Global.Instance.DataBase.GetMovieDataCacheTimeSpanAsync().ContinueWith((movieCachedDataResult) =>
            {
                if (movieCachedDataResult.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    var actualDate = DateTime.Now;
                    var movieCacheDateTimeSpan = movieCachedDataResult.Result;

                    var isBetweenTimeSpan = movieCacheDateTimeSpan != null && actualDate > movieCacheDateTimeSpan.DateFrom && actualDate < movieCacheDateTimeSpan.DateTo;

                    if (isBetweenTimeSpan)
                    {
                        Global.Instance.DataBase.GetMoviesAsync().ContinueWith((moviesCachedResult) =>
                        {
                            if (moviesCachedResult.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                            {
                                var moviesCached = moviesCachedResult.Result;
                                Movies.Clear();
                                PopulateMoviesListView(moviesCached);
                                _movieParameter.Page = 1;
                                _movieParameter.Resource = MoviesApiResourcesConsts.UPCOMING_MOVIES;
                            }
                        });
                    }
                    else
                    {
                        GetUpComingMovies();
                    }
                    this.IsVisibleMovies = true;
                }
            });
        }

        void GetUpComingMovies()
        {
            _movieParameter.Resource = MoviesApiResourcesConsts.UPCOMING_MOVIES;
            _movieParameter.Page = 1;
            Device.BeginInvokeOnMainThread(() =>
            {
                _movieService.GetMovies(_movieParameter).ContinueWith((moviesList) =>
                            {
                                if (moviesList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                                {
                                    var movies = moviesList.Result;
                                    if (!movies.Any())
                                    {
#if __ANDROID__
                                Global.Instance.Toast.ShortToast("No movies found...");
#endif
                                    }
                                    else
                                    {
                                        Movies.Clear();
                                        PopulateMoviesListView(movies);
                                        SetMovieCacheTimeSpanData();
                                    }
                                }
                            });
            });
        }

        void GetMovie(MovieModel movie)
        {
            _Navigated = true;
            _navigation.PushModalAsync(new MoviePage { MovieID = movie.Id }, true);
        }

        void SearchMovie()
        {
            _navigation.PushAsync(new SearchMoviesResultPage { SearchText = this.SearchText }, true);
            this.SearchText = string.Empty;
        }

        void PullToRefresh()
        {
#if __ANDROID__
            Global.Instance.Toast.ShortToast("Refreshing...");
#endif

            this.IsRefreshing = true;
            this.IsVisibleMovies = false;
            _movieParameter.Resource = MoviesApiResourcesConsts.UPCOMING_MOVIES;
            _movieParameter.Page = 1;
            Device.BeginInvokeOnMainThread(() =>
            {
                _movieService.GetMovies(_movieParameter).ContinueWith((moviesList) =>
                    {
                        if (moviesList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        {
                            var movies = moviesList.Result;
                            if (!movies.Any())
                            {
#if __ANDROID__
                                Global.Instance.Toast.ShortToast("No movies found...");
#endif
                            }
                            else
                            {
                                Movies.Clear();
                                PopulateMoviesListView(movies);
                                UpdateMovieCacheTimeSpanData();
                            }
                            IsRefreshing = false;
                            IsVisibleMovies = true;
                        }
                    });
            });
        }

        void MovieAppear(MovieModel movie)
        {
            if (!movie.Position.Equals(Movies.Count - 2))
            {
                return;
            }

            var resource = MoviesApiResourcesConsts.UPCOMING_MOVIES;
            _movieParameter.Page = _movieParameter.Page + 1;
            _movieParameter.Resource = resource;

            Device.BeginInvokeOnMainThread(() =>
            {
                _movieService.GetMovies(_movieParameter).ContinueWith((moviesList) =>
                    {
                        if (moviesList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        {
                            var movies = moviesList.Result;
                            if (!movies.Any())
                            {
#if __ANDROID__
                                Global.Instance.Toast.ShortToast("No more movies...");
#endif
                                return;
                            }
                            PopulateMoviesListView(movies);
                        }
                    });
            });
        }

        void PopulateMoviesListView(List<MovieModel> movies)
        {
            var position = Movies.Count();
            movies.ForEach(movie =>
            {
                position = position + 1;
                movie.Position = position;
                Movies.Add(movie);
            });
        }

        void SetMovieCacheTimeSpanData()
        {
            var movieCacheTimeSpan = DateTime.Now;
            Global.Instance.DataBase.InsertMovieDataCacheTimeSpanAsync(movieCacheTimeSpan, movieCacheTimeSpan.AddDays(1)).ContinueWith((result) =>
            {
                if (result.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    Global.Instance.DataBase.InsertMoviesAsync(Movies.ToList()).Wait();
                }
            });
        }

        void UpdateMovieCacheTimeSpanData()
        {
            Global.Instance.DataBase.DeleteMoviesAsync().ContinueWith((resultDelete) =>
            {
                if (resultDelete.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    var movieCacheTimeSpan = DateTime.Now;
                    Global.Instance.DataBase.UpdateMovieDataCacheTimeSpanAsync(movieCacheTimeSpan, movieCacheTimeSpan.AddDays(1)).ContinueWith((resultUpdate) =>
                    {
                        Global.Instance.DataBase.InsertMoviesAsync(Movies.ToList()).Wait();
                    });
                }
            });
        }
    }
}
