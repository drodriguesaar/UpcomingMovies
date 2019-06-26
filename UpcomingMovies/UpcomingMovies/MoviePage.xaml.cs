using System;
using UpcomingMovies.Dependency;
using UpcomingMovies.Infra;
using UpcomingMovies.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviePage : ContentPage
    {
        public int MovieID { get; set; }
        public MoviePage()
        {
            InitializeComponent();
            SetConfiguration();
            SetPageData();
        }
        void SetConfiguration()
        {
            Util.SetPagePadding(this);
        }
        void SetPageData()
        {
            BindingContext = new MovieDetailViewModel();
        }
        protected override void OnAppearing()
        {
            try
            {
                ((MovieDetailViewModel)BindingContext).GetMovieDetails(MovieID);
            }
            catch (Exception)
            {
                Global.Instance.Toast.ShortToast("Heck, this was unexpected...");
            }
            base.OnAppearing();
        }
    }
}