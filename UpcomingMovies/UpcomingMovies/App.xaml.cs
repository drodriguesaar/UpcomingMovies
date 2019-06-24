using System;
using System.Collections.Generic;
using System.IO;
using UpcomingMovies.DataBase;
using UpcomingMovies.Dependency;
using UpcomingMovies.Model;
using UpcomingMovies.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace UpcomingMovies
{
    public partial class App : Application
    {
        static MovieDataBase dataBase;
        static IToast toast;
        static IService baseService;
        
        public static MovieDataBase DataBase
        {
            get
            {
                if (dataBase == null)
                {
                    dataBase = new MovieDataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MovieCache.db3"));
                }
                return dataBase;
            }
        }
        public static IToast Toast
        {
            get
            {
                if (toast == null)
                {
                    toast = DependencyService.Get<IToast>();
                }
                return toast;
            }
        }
        public static IService BaseService
        {
            get
            {
                if (baseService == null)
                {
                    baseService = ServiceFactory.GetService();
                }
                return baseService;
            }
        }

        public App()
        {
            InitializeComponent();

            var navigationPage = new NavigationPage(new UpcomingMoviesPage());
            navigationPage.BarBackgroundColor = Color.FromHex("#431771");
            navigationPage.BarTextColor = Color.FromHex("#ffffff");
            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
