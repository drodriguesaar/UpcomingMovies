using UpcomingMovies.Infra;
using UpcomingMovies.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CameraCapturePage : ContentPage
    {
        public string MovieUri { get; set; }
        public CameraCapturePage()
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
            BindingContext = new CameraCaptureViewModel(this.Navigation);
        }
        protected override void OnAppearing()
        {
            try
            {
                ((CameraCaptureViewModel)BindingContext).Init();
            }
            catch
            {
                Global.Instance.Toast.Show("Heck, this was unexpected...", false);
            }
            base.OnAppearing();
        }
    }
}