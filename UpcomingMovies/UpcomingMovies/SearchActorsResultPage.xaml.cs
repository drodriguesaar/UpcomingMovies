using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            BindingContext = new SearchActorsResultViewModel(Navigation);
        }
        protected override void OnAppearing()
        {
            try
            {
                ((SearchActorsResultViewModel)BindingContext).SearchByText(this.SearchText);
            }
            catch (Exception)
            {
                Global.Instance.Toast.ShortToast("Heck, this was unexpected...");
            }
            base.OnAppearing();
        }
    }
}