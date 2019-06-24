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
    public partial class SearchResultPage : ContentPage
    {
        public string SearchText { get; set; }

        public SearchResultPage()
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
            BindingContext = new SearchResultViewModel(Navigation);
        }
        protected override void OnAppearing()
        {
            try
            {
                ((SearchResultViewModel)BindingContext).SearchByText(this.SearchText);
            }
            catch (Exception)
            {
                App.Toast.ShortToast("Heck, this was unexpected...");
            }
            base.OnAppearing();
        }
    }
}