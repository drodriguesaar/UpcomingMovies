
using Android.App;
using Android.OS;

namespace UpcomingMovies.Droid
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/MainTheme.Splash")]
    public class SplashScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            StartActivity(typeof(MainActivity));
        }
    }
}