using UpcomingMovies.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpcomingMoviesPage : ContentPage
    {
        public UpcomingMoviesPage()
        {   
            InitializeComponent();
            SetConfiguration();
            SetPageData();
        }
        void SetConfiguration()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Padding = new Thickness(0, 0, 0, 0);
                    break;
            }

        }
        void SetPageData()
        {
            this.BindingContext = new UpcomingMoviesViewModel(this.Navigation);
        }

        protected override void OnAppearing()
        {
            ((UpcomingMoviesViewModel)this.BindingContext).GetUpComingMovies();
            base.OnAppearing();
        }
    }
}