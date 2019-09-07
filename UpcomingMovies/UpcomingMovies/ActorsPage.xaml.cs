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
            BindingContext = new ActorsViewModel(this.Navigation);
        }

        protected override void OnAppearing()
        {
            try
            {
                ((ActorsViewModel)BindingContext).Init();
            }
            catch
            {
                Global.Instance.Toast.Show("Heck, this was unexpected...");
            }
            base.OnAppearing();
        }
    }
}