using UpcomingMovies.Infra;
using UpcomingMovies.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActorsPage : ContentPage
    {
        public ActorsPage()
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
            BindingContext = new ActorsViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            ((ActorsViewModel)BindingContext).Init();
            base.OnAppearing();
        }
    }
}