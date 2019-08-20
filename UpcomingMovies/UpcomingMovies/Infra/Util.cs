using Xamarin.Forms;

namespace UpcomingMovies.Infra
{
   public static class Util
    {
        public static void SetPagePadding(Page page)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    page.Padding = new Thickness(5, 20, 5, 5);
                    break;
                case Device.Android:
                    page.Padding = new Thickness(5, 10, 5, 5);
                    break;
            }
        }
    }
}
