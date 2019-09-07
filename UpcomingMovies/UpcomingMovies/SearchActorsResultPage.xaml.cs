using UpcomingMovies.Infra;
using UpcomingMovies.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchActorsResultPage : ContentPage
    {
        public string SearchText { get; set; }

        public SearchActorsResultPage()
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
            BindingContext = new SearchActorsResultViewModel(this.Navigation);
        }
        protected override void OnAppearing()
        {
            try
            {
                ((SearchActorsResultViewModel)BindingContext).SearchByText(this.SearchText);
            }
            catch
            {
                Global.Instance.Toast.Show("Heck, this was unexpected...");
            }
            base.OnAppearing();
        }
    }
}