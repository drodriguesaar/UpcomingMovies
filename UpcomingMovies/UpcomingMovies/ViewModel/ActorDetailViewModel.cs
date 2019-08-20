using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UpcomingMovies.Consts;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using UpcomingMovies.Service;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class ActorDetailViewModel : ViewModelBase
    {
        ActorModel _Actor;
        bool _IsVisible;
        PeopleService _peopleService;
        MovieParameter _movieParameter;


        public ActorDetailViewModel()
        {
            _peopleService = new PeopleService();
            _movieParameter = new MovieParameter();
        }

        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                if (_IsVisible != value)
                {
                    _IsVisible = value;
                    OnPropertyChanged("IsVisible");
                }
            }
        }

        public ActorModel Actor
        {
            get { return _Actor; }
            set
            {
                if (_Actor != value)
                {
                    _Actor = value;
                    OnPropertyChanged("Actor");
                }
            }
        }
        public void GetActorDetail(int id)
        {
            IsVisible = false;
            _movieParameter.Id = id;
            _movieParameter.Resource = MoviesApiResourcesConsts.SEARCH_PERSON;
            Device.BeginInvokeOnMainThread(() =>
            {
                _peopleService.GetActor(_movieParameter).ContinueWith((actor) =>
                    {
                        if (actor.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        {
                            Actor = actor.Result;
                            IsVisible = true;
                        }
                    });
            });
        }
    }
}
