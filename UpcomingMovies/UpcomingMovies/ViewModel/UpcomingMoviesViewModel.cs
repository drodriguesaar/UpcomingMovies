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
using UpcomingMovies.Service;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class UpcomingMoviesViewModel : INotifyPropertyChanged
    {
        readonly INavigation _navigation;
        readonly MovieService _movieService;
        readonly GenreService _genreService;
        readonly PeopleService _peopleService;
        readonly MovieParameter _movieParameter;

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
        public ObservableCollection<MovieModel> NowPlaying
        {
            get { return _NowPlaying; }
            set
            {
                if (_NowPlaying != value)
                {
                    _NowPlaying = value;
                    OnPropertyChanged("NowPlaying");
                }
            }
        }
        public ObservableCollection<GenreModel> Genres
        {
            get { return _Genres; }
            set
            {
                if (_Genres != value)
                {
                    _Genres = value;
                    OnPropertyChanged("Genres");
                }
            }
        }
        public ObservableCollection<ActorModel> Actors
        {
            get { return _Actors; }
            set
            {
                if (_Actors != value)
                {
                    _Actors = value;
                    OnPropertyChanged("Actors");
                }
            }
        }

        bool _IsReady;
        bool _IsVisible;
        bool _IsRefreshing;
        string _SearchText;
        ObservableCollection<MovieModel> _Movies;
        ObservableCollection<MovieModel> _NowPlaying;
        ObservableCollection<GenreModel> _Genres;
        ObservableCollection<ActorModel> _Actors;
        public UpcomingMoviesViewModel()
        {
        }
        public UpcomingMoviesViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _movieService = new MovieService();
            _genreService = new GenreService();
            _peopleService = new PeopleService();
            _movieParameter = new MovieParameter();

            Movies = new ObservableCollection<MovieModel>();
            Genres = new ObservableCollection<GenreModel>();
            NowPlaying = new ObservableCollection<MovieModel>();
            Actors = new ObservableCollection<ActorModel>();

            GetMovieCommand = new Command<MovieModel>(GetMovie);
            SearchMovieCommand = new Command(SearchMovie);
            MovieAppearCommand = new Command<MovieModel>(MovieAppear);
            PullToRefreshCommand = new Command(PullToRefresh);
            GetMoviesByGenreCommand = new Command<GenreModel>(GetMoviesByGenre);
        }

        public ICommand GetMovieCommand { get; set; }
        public ICommand GetMoviesByGenreCommand { get; set; }
        public ICommand SearchMovieCommand { get; set; }
        public ICommand MovieAppearCommand { get; set; }
        public ICommand PullToRefreshCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Movies region
        void SearchMovie()
        {
            NavigatedToDetails = true;
            _navigation.PushAsync(new SearchResultPage { SearchText = this.SearchText });
            this.SearchText = string.Empty;
        }
        void PullToRefresh()
        {
#if __ANDROID__
            App.Toast.ShortToast("Refreshing...");
#endif

            this.IsRefreshing = true;
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
                        }
                        else
                        {
                            Movies.Clear();
                            PopulateMoviesListView(movies);
                            UpdateMovieCacheTimeSpanData();
                        }
                        IsRefreshing = false;
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
                        PopulateMoviesListView(movies);
                    }
                });
            });
        }

        public void GetMovies()
        {
            this.IsVisible = false;
            App.DataBase.GetMovieDataCacheTimeSpanAsync().ContinueWith((movieCachedDataResult) =>
            {
                if (movieCachedDataResult.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
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
                    this.IsVisible = true;
                }
            });
        }
        void GetUpComingMovies()
        {
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
            NavigatedToDetails = true;
            _navigation.PushAsync(new MoviePage { MovieID = movie.Id });
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
            App.DataBase.InsertMovieDataCacheTimeSpanAsync(movieCacheTimeSpan, movieCacheTimeSpan.AddDays(1)).ContinueWith((result) =>
            {
                if (result.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    App.DataBase.InsertMoviesAsync(Movies.ToList()).Wait();
                }
            });
        }
        void UpdateMovieCacheTimeSpanData()
        {
            App.DataBase.DeleteMoviesAsync().ContinueWith((resultDelete) =>
            {
                if (resultDelete.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                {
                    var movieCacheTimeSpan = DateTime.Now;
                    App.DataBase.UpdateMovieDataCacheTimeSpanAsync(movieCacheTimeSpan, movieCacheTimeSpan.AddDays(1)).ContinueWith((resultUpdate) =>
                    {
                        App.DataBase.InsertMoviesAsync(Movies.ToList()).Wait();
                    });
                }
            });
        } 
        #endregion

        #region Now playing region
        public void GetNowPlaying()
        {
            _movieParameter.Resource = MoviesApiResourcesConsts.NOW_PLAYING;
            _movieParameter.Page = 1;
            _movieService.GetMovies(_movieParameter).ContinueWith((nowPlayingList) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (nowPlayingList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var movies = nowPlayingList.Result;
                        if (!movies.Any())
                        {
                            App.Toast.ShortToast("No movies found...");

                        }
                        else
                        {
                            NowPlaying.Clear();
                            PopulateNowPlayingListView(movies);
                        }
                    }
                });
            });
        }
        void PopulateNowPlayingListView(List<MovieModel> nowPlayingList)
        {
            nowPlayingList.ForEach(nowplaying =>
            {
                NowPlaying.Add(nowplaying);
            });
        } 
        #endregion

        #region Genres region
        public void GetGenres()
        {
            _genreService.GetGenres().ContinueWith((genresList) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (genresList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var genres = genresList.Result;
                        Genres.Clear();
                        PopulateGenresListView(genres);
                    }
                });
            });
        }
        void GetMoviesByGenre(GenreModel genre)
        {

        } 
        void PopulateGenresListView(List<GenreModel> genres)
        {
            genres.ForEach(genre =>
            {
                Genres.Add(genre);
            });
        }
        #endregion

        #region Actors region
        public void GetActors()
        {
            _peopleService.GetActors().ContinueWith((actorsList) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (actorsList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var actors = actorsList.Result;
                        Actors.Clear();
                        PopulateActorsListView(actors);
                    }
                });
            });
        }
        void PopulateActorsListView(List<ActorModel> actors)
        {
            actors.ForEach(actor =>
            {
                Actors.Add(actor);
            });
        } 
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
