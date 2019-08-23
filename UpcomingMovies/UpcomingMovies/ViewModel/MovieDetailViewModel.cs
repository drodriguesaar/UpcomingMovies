﻿using System.Threading.Tasks;
using System.Windows.Input;
using UpcomingMovies.Component;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class MovieDetailViewModel : ViewModelBase
    {
        readonly MovieService _movieService;
        readonly MovieParameter _movieParameter;
        
        public MovieDetailViewModel()
        {

        }

        public MovieDetailViewModel(INavigation navigation)
        {
            _Navigation = navigation;
            _movieParameter = new MovieParameter();
            _movieService = new MovieService();
            CloseModalCommand = new Command(CloseModal);
        }

        MovieModel _Movie;
        public MovieModel Movie
        {
            get { return _Movie; }
            set
            {
                if (_Movie != value)
                {
                    _Movie = value;
                    OnPropertyChanged("Movie");
                }
            }
        }

        public ICommand CloseModalCommand { get; set; }

        void CloseModal()
        {
            this._Navigation.PopModalAsync(true);
        }

        public void GetMovieDetails(int id)
        {
            
            _movieParameter.Id = id;
            Device.BeginInvokeOnMainThread(() =>
            {
                _movieService.GetMovie(_movieParameter).ContinueWith((movie) =>
                    {
                        if (movie.Status == TaskStatus.RanToCompletion)
                        {
                            Movie = movie.Result;
                        }
                    });
            });
        }
    }
}
