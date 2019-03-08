using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using UpcomingMovies.Component;
using UpcomingMovies.Consts;
using UpcomingMovies.Dependency;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpcomingMoviesPage : ContentPage
    {
        MovieComponent _movieComponent;
        MovieParameter _movieParameter;

        #region Properties
        public ObservableCollection<MovieModel> MovieListObservableCollection { get; set; }
        public int TotalPages { get; set; }
        #endregion

        public UpcomingMoviesPage()
        {
            MovieListObservableCollection = new ObservableCollection<MovieModel>();
            _movieComponent = new MovieComponent();
            _movieParameter = new MovieParameter();
            _movieParameter.Page = 1;
            InitializeComponent();
            SetConfiguration();
            SetEvents();
            SetPageData();
        }

        #region Page Events
        async void SetPageData()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    _movieComponent.Resource = string.IsNullOrEmpty(_movieParameter.Query) ? MoviesApiResourcesConsts.UPCOMING_MOVIES : MoviesApiResourcesConsts.SEARCH_MOVIE;
                    var result = await _movieComponent.GetMovies(_movieParameter);
                    if (!result.Movies.Any())
                    {
                        DependencyService.Get<IToast>().ShortToast("No movies found...");
                        return;
                    }
                    PopulateListView(result);
                }
                catch
                {
                    DependencyService.Get<IToast>().ShortToast("Heck, this was unexpected...");
                }
            });


        }
        void SetEvents()
        {
            btnSearch.Clicked += BtnSearch_Clicked;
            lstMovies.ItemTapped += LstMovies_ItemTapped;
            lstMovies.ItemAppearing += LstMovies_ItemAppearing;
            sbMovieSearch.TextChanged += SbMovieSearch_TextChanged;
        }
        void SetConfiguration()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    Padding = new Thickness(0, 5, 0, 0);
                    break;
            }
        }
        void PopulateListView(UpcomingMovieModel upcomingMovieModel)
        {
            try
            {
                TotalPages = upcomingMovieModel.Total;
                var postion = MovieListObservableCollection.Count;

                upcomingMovieModel.Movies.ForEach(movie =>
                {
                    movie.Position = (postion++);
                    MovieListObservableCollection.Add(movie);
                });
                lstMovies.ItemsSource = MovieListObservableCollection;
            }
            catch
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().ShortToast("Heck, this was unexpected...");
                });
            }
        }
        #endregion

        #region Control Events
        async void SbMovieSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var isSearchTextEmpty = string.IsNullOrEmpty(sbMovieSearch.Text);
                if (!isSearchTextEmpty)
                {
                    return;
                }
                _movieParameter.Page = 1;
                _movieParameter.Query = string.Empty;
                MovieListObservableCollection.Clear();
                lstMovies.ItemsSource = MovieListObservableCollection;
                SetPageData();
            }
            catch
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().ShortToast("Heck, this was unexpected...");
                });
            }
        }
        async void BtnSearch_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                var isSearchTermReady = !string.IsNullOrEmpty(sbMovieSearch.Text) && sbMovieSearch.Text.Length > 3;
                if (!isSearchTermReady)
                {
                    return;
                }

                DependencyService.Get<IToast>().ShortToast("Hang on...");

                _movieParameter.Query = HttpUtility.UrlEncode(sbMovieSearch.Text);
                _movieParameter.Page = 1;
                _movieComponent.Resource = MoviesApiResourcesConsts.SEARCH_MOVIE;

                var result = await _movieComponent.GetMovies(_movieParameter);
                if (!result.Movies.Any())
                {
                    DependencyService.Get<IToast>().ShortToast("No movies found...");
                    return;
                }

                MovieListObservableCollection.Clear();
                PopulateListView(result);
            }
            catch
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().ShortToast("Heck, this was unexpected...");
                });
            }
        }
        async void LstMovies_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var movieTapped = (MovieModel)e.Item;
                if (movieTapped == null)
                {
                    return;
                }
                await Navigation.PushAsync(new MoviePage { MovieID = movieTapped.Id });
            }
            catch
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().ShortToast("Heck, this was unexpected...");
                });
            }
        }
        void LstMovies_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                var movieAppeared = (MovieModel)e.Item;
                if (movieAppeared.Position.Equals(MovieListObservableCollection.Count - 3))
                {
                    _movieParameter.Page = _movieParameter.Page + 1;
                    if (_movieParameter.Page > TotalPages)
                    {
                        return;
                    }
                    SetPageData();
                }
            }
            catch
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    DependencyService.Get<IToast>().ShortToast("Heck, this was unexpected...");
                });
            }
        }
        #endregion
    }
}