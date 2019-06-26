using UpcomingMovies.Infra;
using UpcomingMovies.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActorPage : ContentPage
    {
        public int ActorID { get; set; }
        public ActorPage()
        {
            InitializeComponent();
            SetConfiguration();
            SetPageData(); ;
        }
        void SetConfiguration()
        {
            Util.SetPagePadding(this);
        }
        void SetPageData()
        {
            BindingContext = new ActorDetailViewModel();
        }
        protected override void OnAppearing()
        {
            try
            {
                ((ActorDetailViewModel)BindingContext).GetActorDetail(ActorID);
            }
            catch
            {
                Global.Instance.Toast.ShortToast("Heck, this was unexpected...");
            }
            base.OnAppearing();
        }
    }
}