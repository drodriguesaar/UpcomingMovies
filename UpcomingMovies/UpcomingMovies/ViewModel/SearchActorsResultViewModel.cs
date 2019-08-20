using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Input;
using UpcomingMovies.Consts;
using UpcomingMovies.Infra;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
using UpcomingMovies.Service;
using Xamarin.Forms;

namespace UpcomingMovies.ViewModel
{
    public class SearchActorsResultViewModel : ViewModelBase
    {
        readonly PeopleService _peopleService;
        readonly MovieParameter _movieParameter;
        readonly INavigation _navigation;

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

        bool _IsVisible;
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

        public SearchActorsResultViewModel()
        {


        }
        public SearchActorsResultViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _movieParameter = new MovieParameter();
            _peopleService = new PeopleService();
            Actors = new ObservableCollection<ActorModel>();

            GetActorCommand = new Command<ActorModel>(GetActor);
            ActorAppearCommand = new Command<ActorModel>(ActorAppear);
        }

        public ICommand GetActorCommand { get; set; }
        public ICommand ActorAppearCommand { get; set; }


        public void SearchByText(string searchText)
        {
            this.SearchText = string.Format("Results to {0}", searchText);
            this.IsVisible = false;

            Global.Instance.Toast.ShortToast(string.Format("Searching by {0}...", searchText));

            _movieParameter.Page = 1;
            _movieParameter.Query = HttpUtility.UrlEncode(searchText);
            _movieParameter.Resource = MoviesApiResourcesConsts.SEARCH_PERSON;

            Device.BeginInvokeOnMainThread(() =>
            {
                _peopleService.GetActors(_movieParameter).ContinueWith((actorsList) =>
                    {
                        if (actorsList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                        {
                            var actors = actorsList.Result;
                            if (!actors.Any())
                            {
                                Global.Instance.Toast.ShortToast("No actors found...");
                            }
                            else
                            {
                                Actors.Clear();
                                PopulateListView(actors);
                            }
                            this.IsVisible = true;
                        }
                    });
            });

        }

        void GetActor(ActorModel actorModel)
        {
            _Navigated = true;
            _navigation.PushModalAsync(new ActorPage { ActorID = actorModel.ID }, true);
        }

        void ActorAppear(ActorModel actorModel)
        {
            if (!actorModel.Position.Equals(Actors.Count - 2))
            {
                return;
            }

            var resource = MoviesApiResourcesConsts.SEARCH_PERSON;
            var searchQuery = HttpUtility.UrlEncode(SearchText);

            _movieParameter.Page = _movieParameter.Page + 1;
            _movieParameter.Resource = resource;
            _movieParameter.Query = searchQuery;

            Device.BeginInvokeOnMainThread(() =>
            {
                _peopleService.GetActors(_movieParameter).ContinueWith((actorsList) =>
                {

                    if (actorsList.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        var actors = actorsList.Result;
                        if (!actors.Any())
                        {
                            Global.Instance.Toast.ShortToast("No more actors...");
                            return;
                        }
                        PopulateListView(actors);
                    }

                });
            });
        }

        void PopulateListView(List<ActorModel> actors)
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

