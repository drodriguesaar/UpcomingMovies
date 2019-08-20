using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UpcomingMovies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrincipalTabbedPage : TabbedPage
    {
        public PrincipalTabbedPage()
        {
            InitializeComponent();

            

            var MoviesNavigationPage = new NavigationPage(new MoviesPage())
            {
                BarBackgroundColor = Color.FromHex("#431771"),
                BarTextColor = Color.FromHex("#ffffff"),
                IconImageSource = "ic_cinema.png",
                Title = "Movies",
            };

            

            var ActorsNavigationPage = new NavigationPage(new ActorsPage())
            {
                BarBackgroundColor = Color.FromHex("#431771"),
                BarTextColor = Color.FromHex("#ffffff"),
                IconImageSource = "ic_actors.png",
                Title = "Actors"
            };


            this.BarTextColor = Color.FromHex("#ffffff");
            this.BarBackgroundColor = Color.FromHex("#431771");
            this.SelectedTabColor = Color.FromHex("#7b43b5");
            this.IconImageSource = ImageSource.FromFile("icon.png");
            this.Title = "Upcoming Movies";


            Children.Add(MoviesNavigationPage);
            Children.Add(ActorsNavigationPage);
        }
    }
}