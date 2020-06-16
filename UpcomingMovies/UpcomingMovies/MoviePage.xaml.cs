using UpcomingMovies.Infra;
using UpcomingMovies.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MoviePage : ContentPage
    {
        //Prop
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
            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, false);
        }
        void SetPageData()
        {
            BindingContext = new MovieDetailViewModel(this.Navigation);
        }
        protected override void OnAppearing()
        {
            try
            {
                ((MovieDetailViewModel)BindingContext).GetMovieDetails(MovieID);
            }
            catch
            {
                Global.Instance.Toast.Show("Heck, this was unexpected...");
            }
            base.OnAppearing();
        }
    }
}
