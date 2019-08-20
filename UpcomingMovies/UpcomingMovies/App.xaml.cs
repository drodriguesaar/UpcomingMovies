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
        public App()
        {
            InitializeComponent();

            MainPage = new PrincipalTabbedPage();
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
