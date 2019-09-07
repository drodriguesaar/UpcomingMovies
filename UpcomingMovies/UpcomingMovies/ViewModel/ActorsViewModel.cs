using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UpcomingMovies.Consts;
using UpcomingMovies.Infra;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using UpcomingMovies.Service;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class ActorsViewModel : ViewModelBase
    {
        readonly PeopleService _peopleService;
        readonly MovieParameter _movieParameter;

        string _SearchText;
        public string SearchText
        {
            get { return _SearchText; }
            set
            {
                if (_SearchText != value)
                {
                    _SearchText = value;
                    OnPropertyChanged("SearchText");
                }
            }
        }

        bool _IsVisileActors;
        public bool IsVisileActors
        {
            get { return _IsVisileActors; }
            set
            {
                if (_IsVisileActors != value)
                {
                    _IsVisileActors = value;
                    OnPropertyChanged("IsVisileActors");
                }
            }
        }

        bool _IsRefreshing;
        public bool IsRefreshing
        {
            get { return _IsRefreshing; }
            set
            {
                if (_IsRefreshing != value)
                {
                    _IsRefreshing = value;
                    OnPropertyChanged("IsRefreshing");
                }
            }
        }

        ObservableCollection<ActorModel> _Actors;
        public ObservableCollection<ActorModel> Actors
        {
            get { return _Actors; }
            set
            {
                if (_Actors != value)
                {
                    _Actors = value;
                    OnPropertyChanged("Actors");
                }
            }
        }

        public ActorsViewModel()
        {
        }

        public ActorsViewModel(INavigation navigation)
        {
            _Navigation = navigation;
            _peopleService = new PeopleService();
            _movieParameter = new MovieParameter();
            Actors = new ObservableCollection<ActorModel>();

            SearchActorCommand = new Command(SearchActor);
            PullToRefreshCommand = new Command(PullToRefresh);
            GetActorCommand = new Command<ActorModel>(GetActor);
            ActorAppearCommand = new Command<ActorModel>(ActorAppear);
        }

        public ICommand SearchActorCommand { get; set; }
        public ICommand GetActorCommand { get; set; }
        public ICommand ActorAppearCommand { get; set; }
        public ICommand PullToRefreshCommand { get; set; }
        
        public void Init()
        {
            if (_Navigated)
            {
                _Navigated = false;
                return;
            }

            this.IsVisileActors = false;
            Device.BeginInvokeOnMainThread(() =>
            {
                _movieParameter.Resource = MoviesApiResourcesConsts.POPULAR_ACTORS;
                _movieParameter.Page = 1;
                _peopleService.GetActors(_movieParameter).ContinueWith((actorsList) =>
                {
                    if (actorsList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var actorsResult = actorsList.Result;
                        if (!actorsResult.Any())
                        {
                            Global.Instance.Toast.Show("No actors found...");
                        }
                        else
                        {
                            Actors.Clear();
                            PopulateActorListView(actorsResult);
                            this.IsVisileActors = true;
                        }
                    }
                });
            });
        }
        void SearchActor()
        {
            _Navigated = true;
            _Navigation.PushModalAsync(new SearchActorsResultPage { SearchText = this.SearchText }, true);
            this.SearchText = string.Empty;
        }
        void PullToRefresh()
        {
            this.IsRefreshing = true;
            _movieParameter.Resource = MoviesApiResourcesConsts.POPULAR_ACTORS;
            _movieParameter.Page = 1;
            Device.BeginInvokeOnMainThread(() =>
            {
                _peopleService.GetActors(_movieParameter).ContinueWith((actorList) =>
                {
                    if (actorList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var actorsResult = actorList.Result;
                        if (!actorsResult.Any())
                        {
                            Global.Instance.Toast.Show("No actors found...");
                        }
                        else
                        {
                            Actors.Clear();
                            PopulateActorListView(actorsResult);
                        }
                        this.IsRefreshing = false;
                    }
                });
            });
        }
        void ActorAppear(ActorModel actor)
        {
            if (!actor.Position.Equals(Actors.Count - 2))
            {
                return;
            }

            var resource = MoviesApiResourcesConsts.POPULAR_ACTORS;

            _movieParameter.Page = _movieParameter.Page + 1;
            _movieParameter.Resource = resource;
            Device.BeginInvokeOnMainThread(() =>
            {
                _peopleService.GetActors(_movieParameter).ContinueWith((actorsList) =>
                {
                    if (actorsList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var actors = actorsList.Result;
                        if (!actors.Any())
                        {
                            Global.Instance.Toast.Show("No more movies...");
                            return;
                        }
                        PopulateActorListView(actors);
                    }
                });
            });
        }
        void GetActor(ActorModel actorModel)
        {
            _Navigated = true;
            _Navigation.PushModalAsync(new ActorPage { ActorID = actorModel.ID }, true);
        }
        void PopulateActorListView(List<ActorModel> actors)
        {
            var position = Actors.Count();
            actors.ForEach(actor =>
            {
                position = position + 1;
                actor.Position = position;
                Actors.Add(actor);
            });
        }
    }
}
