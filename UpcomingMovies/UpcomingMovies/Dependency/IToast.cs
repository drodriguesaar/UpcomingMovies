using Android.Widget;

namespace UpcomingMovies.Dependency
{
    public interface IToast
    {
        void Show(string message, bool shortLength = true);
    }
}
