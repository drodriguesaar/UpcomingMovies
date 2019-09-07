using Plugin.Media;
using Plugin.Media.Abstractions;
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
        readonly MovieService _movieService;
        readonly MovieParameter _movieParameter;
        
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
            _Navigation = navigation;
            _movieService = new MovieService();
            _movieParameter = new MovieParameter();
            Movies = new ObservableCollection<MovieModel>();

            GetMovieCommand = new Command<MovieModel>(GetMovie);
            SearchMovieCommand = new Command(SearchMovie);
            MovieAppearCommand = new Command<MovieModel>(MovieAppear);
            PullToRefreshCommand = new Command(PullToRefresh);
            CameraCaptureCommand = new Command(CameraCapture);
        }


        public ICommand GetMovieCommand { get; set; }
        public ICommand SearchMovieCommand { get; set; }
        public ICommand MovieAppearCommand { get; set; }
        public ICommand PullToRefreshCommand { get; set; }
        public ICommand CameraCaptureCommand { get; set; }

        public void Init()
        {
            if (_Navigated)
            {
                _Navigated = false;
                return;
            }

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
                            Global.Instance.Toast.Show("No movies found...");
                        }
                        else
                        {
                            Movies.Clear();
                            PopulateMoviesListView(movies);
                            this.IsVisibleMovies = true;
                        }
                    }
                });
            });
        }

        void GetMovie(MovieModel movie)
        {
            _Navigated = true;
            _Navigation.PushModalAsync(new MoviePage { MovieID = movie.Id }, true);
        }

        void SearchMovie()
        {
            _Navigated = true;
            _Navigation.PushModalAsync(new SearchMoviesResultPage { SearchText = this.SearchText }, true);
            this.SearchText = string.Empty;
        }

        void CameraCapture()
        {
            _Navigated = true;
            this.SearchText = string.Empty;
            _Navigation.PushModalAsync(new CameraCapturePage { }, true);
        }

        void PullToRefresh()
        {
            Global.Instance.Toast.Show("Refreshing...");

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
                                Global.Instance.Toast.Show("No movies found...");
                            }
                            else
                            {
                                Movies.Clear();
                                PopulateMoviesListView(movies);
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
                                Global.Instance.Toast.Show("No more movies...");
                            }
                            else
                            {
                                PopulateMoviesListView(movies);
                            }
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
    }
}
