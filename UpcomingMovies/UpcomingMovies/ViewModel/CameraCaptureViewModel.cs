using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    public class CameraCaptureViewModel : ViewModelBase
    {
        readonly MovieService _movieService;
        readonly MovieParameter _movieParameter;
        public CameraCaptureViewModel()
        {

        }

        public CameraCaptureViewModel(INavigation navigation)
        {
            _Navigation = navigation;
            _movieParameter = new MovieParameter();
            _movieService = new MovieService();
            Movies = new ObservableCollection<MovieModel>();
            GetMovieCommand = new Command<MovieModel>(GetMovie);
            MovieAppearCommand = new Command<MovieModel>(MovieAppear);
            PullToRefreshCommand = new Command(PullToRefresh);
        }

        public async void Init()
        {
            if (this._Navigated)
            {
                _Navigated = false;
                return;
            }

            this.IsVisible = false;
            Global.Instance.Toast.Show("Starting camera...");
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Global.Instance.Toast.Show("No camera available!");
                return;
            }

            var storage = new StoreCameraMediaOptions()
            {
                SaveToAlbum = true,
                Name = string.Format("Movie_Poster_{0}.jpg", Guid.NewGuid().ToString()),
                DefaultCamera = CameraDevice.Rear,
                PhotoSize = PhotoSize.Large,
                ModalPresentationStyle = MediaPickerModalPresentationStyle.OverFullScreen
            };

            var moviePosterMedia = await CrossMedia.Current.TakePhotoAsync(storage);

            if (moviePosterMedia == null)
            {
                Global.Instance.Toast.Show("No photo taken...");
            }

            var text = Global.Instance.CameraOCR.ReadTextFromImage(moviePosterMedia.Path);

            if (string.IsNullOrEmpty(text))
            {
                Global.Instance.Toast.Show("No recognizable text...");
            }

            this.SearchText = text;
            Global.Instance.Toast.Show(string.Format("Searching by {0}...", text));

            _movieParameter.Page = 1;
            _movieParameter.Query = HttpUtility.UrlEncode(text);
            _movieParameter.Resource = MoviesApiResourcesConsts.SEARCH_MOVIE;

            await _movieService.GetMovies(_movieParameter).ContinueWith((moviesList) =>
             {
                 Device.BeginInvokeOnMainThread(() =>
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
                             PopulateListView(movies);
                         }
                         this.IsVisible = true;
                         this.IsRefreshing = false;
                     }
                 });
             });

        }

        bool _IsVisible;
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

        public ICommand GetMovieCommand { get; private set; }

        public ICommand MovieAppearCommand { get; private set; }

        public ICommand PullToRefreshCommand { get; private set; }

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
            _Navigated = true;
            _Navigation.PushModalAsync(new MoviePage { MovieID = movie.Id });
        }

        void MovieAppear(MovieModel movie)
        {
            if (!movie.Position.Equals(Movies.Count - 2))
            {
                return;
            }

            var resource = MoviesApiResourcesConsts.SEARCH_MOVIE;
            var searchQuery = HttpUtility.UrlEncode(SearchText);

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
                            Global.Instance.Toast.Show("No more movies...");
                            return;
                        }
                        PopulateListView(movies);
                    }
                });
            });
        }

        void PullToRefresh()
        {
            this.IsRefreshing = true;
            this.IsVisible = false;
            Init();
        }
    }
}
