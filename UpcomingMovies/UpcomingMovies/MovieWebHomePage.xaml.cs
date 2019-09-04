using UpcomingMovies.Infra;
using UpcomingMovies.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MovieWebHomePage : ContentPage
    {
        public string MovieUri { get; set; }
        public MovieWebHomePage()
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
            BindingContext = new MovieWebHomeViewModel(this.Navigation);
        }
        protected override void OnAppearing()
        {
            try
            {
                ((MovieWebHomeViewModel)BindingContext).Init(MovieUri);
            }
            catch
            {
                Global.Instance.Toast.ShortToast("Heck, this was unexpected...");
            }
            base.OnAppearing();
        }
    }
}