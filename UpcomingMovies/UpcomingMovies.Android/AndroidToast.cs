using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using UpcomingMovies.Dependency;
using UpcomingMovies.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidToast))]
namespace UpcomingMovies.Droid
{

    public class AndroidToast : IToast
    {
        public void Show(string message, bool shortLength = true)
        {
            Toast.MakeText(Application.Context, message, shortLength ? ToastLength.Short : ToastLength.Long).Show();
        }
    }
}