using System.Windows.Input;
using UpcomingMovies.Consts;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using UpcomingMovies.Service;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class ActorDetailViewModel : ViewModelBase
    {
        readonly PeopleService _peopleService;
        readonly MovieParameter _movieParameter;

        public ActorDetailViewModel()
        {
        }

        public ActorDetailViewModel(INavigation navigation)
        {
            _Navigation = navigation;
            _peopleService = new PeopleService();
            _movieParameter = new MovieParameter();
            CloseModalCommand = new Command(CloseModal);
        }
        
        ActorModel _Actor;
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

        public ICommand CloseModalCommand { get; set; }

        void CloseModal()
        { 
            this._Navigation.PopModalAsync(true);
        }

        public void GetActorDetail(int id)
        {
            _movieParameter.Id = id;
            _movieParameter.Resource = MoviesApiResourcesConsts.SEARCH_PERSON;
            Device.BeginInvokeOnMainThread(() =>
            {
                _peopleService.GetActor(_movieParameter).ContinueWith((actor) =>
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
