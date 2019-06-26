using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpcomingMovies.Consts;
using UpcomingMovies.DTO;
using UpcomingMovies.Enums;
using UpcomingMovies.Infra;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
namespace UpcomingMovies.Service
{
    public class PeopleService
    {
        internal async Task<List<ActorModel>> GetActors(MovieParameter movieParameter)
        {
            var actorList = new List<ActorModel>();
            try
            {
                var resource = MoviesApiResourcesConsts.POPULAR_ACTORS;
                var response = await Global.Instance.BaseService.Consume<MovieParameter, ResponseListDTO<List<PeopleDTO>>>(movieParameter, resource, Enums.HTTPMethodEnum.GET);
                actorList = response.results.Select(people => new ActorModel
                {
                    Name = people.name,
                    Photo = people.profile_path.BuildImageURI(),
                    ID = people.id
                }).ToList();
            }
            catch
            {
            }
            return actorList;
        }
        internal async Task<ActorModel> GetActor(MovieParameter movieParameter)
        {
            var actorModel = new ActorModel();
            try
            {
                var resource = string.Format(MoviesApiResourcesConsts.ACTOR, movieParameter.Id);
                var response = await Global.Instance.BaseService.Consume<MovieParameter, PersonDTO>(movieParameter, resource, HTTPMethodEnum.GET);
                actorModel.Name = response.name;
                actorModel.Photo = response.profile_path.BuildImageURI();
                actorModel.ID = response.id;
            }
            catch
            {
                actorModel.Error = true;
                actorModel.ErrorMessage = "Ops, an error occurred while fetchind data from this actor";
            }
            return actorModel;
        }
    }
}
