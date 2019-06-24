using System.Collections;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace UpcomingMovies.CustomViews
{
    public class GenresHorizontalScrollView : ScrollView
    {
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(GenresHorizontalScrollView), default(IEnumerable));

        public IEnumerable ItemsSource
        {
            set { SetValue(ItemsSourceProperty, value); }
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
        }
    }
}
