using UpcomingMovies.Infra;
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
            Util.SetPagePadding(this);
        }
        void SetPageData()
        {
            BindingContext = new UpcomingMoviesViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            ((UpcomingMoviesViewModel)BindingContext).GetMovies();
            base.OnAppearing();
        }
    }
}