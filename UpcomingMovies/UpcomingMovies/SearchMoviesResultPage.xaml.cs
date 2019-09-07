using UpcomingMovies.Infra;
using UpcomingMovies.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchMoviesResultPage : ContentPage
    {
        public string SearchText { get; set; }

        public SearchMoviesResultPage()
        {
            InitializeComponent();
            SetConfiguration();
            SetPageData();
        }
        void SetConfiguration()
        {
            Util.SetPagePadding(this);
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }
        void SetPageData()
        {
            BindingContext = new SearchMoviesResultViewModel(this.Navigation);
        }
        protected override void OnAppearing()
        {
            try
            {
                ((SearchMoviesResultViewModel)BindingContext).SearchByText(this.SearchText);
            }
            catch
            {
                Global.Instance.Toast.Show("Heck, this was unexpected...");
            }
            base.OnAppearing();
        }
    }
}