using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{

    public class MovieWebHomeViewModel : ViewModelBase
    {
        string _MovieUri;
        public string MovieUri
        {
            get { return _MovieUri; }
            set
            {
                if (_MovieUri != value)
                {
                    _MovieUri = value;
                    OnPropertyChanged("MovieUri");
                }
            }
        }

        public MovieWebHomeViewModel()
        {

        }

        public MovieWebHomeViewModel(INavigation navigation)
        {
            this._Navigation = navigation;
            CloseWebViewCommand = new Command(CloseWebView);
        }

        public ICommand CloseWebViewCommand { get; set; }

        public void Init(string movieHomePageUri)
        {
            MovieUri = movieHomePageUri;
        }
        void CloseWebView()
        {
            this._Navigation.PopModalAsync(true);
        }
    }
}
