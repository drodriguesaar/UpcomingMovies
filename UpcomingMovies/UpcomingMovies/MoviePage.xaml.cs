using System;
using UpcomingMovies.Dependency;
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
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    Padding = new Thickness(0, 5, 0, 0);
                    break;
            }
        }
        void SetPageData()
        {
            BindingContext = new MovieDetailViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await ((MovieDetailViewModel)BindingContext).GetMovieDetails(MovieID);
                }
                catch (Exception)
                {
                    DependencyService.Get<IToast>().ShortToast("Heck, this was unexpected...");
                }
            });
        }
    }
}