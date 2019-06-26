using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using UpcomingMovies.Service;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class ActorDetailViewModel : ViewModelBase
    {
        ActorModel _Actor;
        PeopleService _peopleService;
        MovieParameter _movieParameter;
        public ActorDetailViewModel()
        {
            _peopleService = new PeopleService();
            _movieParameter = new MovieParameter();
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
            _movieParameter.Id = id;
            _peopleService.GetActor(_movieParameter).ContinueWith((actor) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (actor.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        Actor = actor.Result;
                    }
                });
            });
        }
    }
}
