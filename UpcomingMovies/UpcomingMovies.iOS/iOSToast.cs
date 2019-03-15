using Foundation;
using UIKit;
using UpcomingMovies.Dependency;
using UpcomingMovies.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(iOSToast))]
namespace UpcomingMovies.iOS
{
    public class iOSToast : IToast
    {
        NSTimer alertDelay;
        UIAlertController alert;

        public void ShortToast(string message)
        {
            alertDelay = NSTimer.CreateScheduledTimer(3.5, (obj) =>
            {
                DismissToast();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
        void DismissToast()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}