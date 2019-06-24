using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UpcomingMovies.Consts;
using UpcomingMovies.DTO;
using UpcomingMovies.Model;
using UpcomingMovies.Parameter;
namespace UpcomingMovies.Service
{
    public class PeopleService
    {
        internal async Task<List<ActorModel>> GetActors()
        {
            var actorList = new List<ActorModel>();
            try
            {
                var movieParameter = new MovieParameter();
                var resource = MoviesApiResourcesConsts.POPULAR_ACTORS;
                var response = await App.BaseService.Consume<MovieParameter, ResponseListDTO<List<PeopleDTO>>>(movieParameter, resource, Enums.HTTPMethodEnum.GET);
                actorList = response.results.Select(people => new ActorModel { Name = people.name, Photo = BuildPosterUri(people.profile_path) }).ToList();
            }
            catch
            {
            }
            return actorList;
        }
        string BuildPosterUri(string posterPath, string size = "200")
        {
            if (string.IsNullOrEmpty(posterPath))
            {
                return "https://static.thenounproject.com/png/193751-200.png";
            }

            return string.Format("https://image.tmdb.org/t/p/w{1}/{0}", posterPath, size);
        }
    }
}
