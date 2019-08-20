using System;
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
                var resource = movieParameter.Resource;
                var response = await Global.Instance.BaseService.Consume<MovieParameter, ResponseListDTO<List<PeopleDTO>>>(movieParameter, resource, Enums.HTTPMethodEnum.GET);
                actorList = response.results.Select(people => new ActorModel
                {
                    Name = people.name,
                    Photo = people.profile_path.BuildImageURI(),
                    ID = people.id,
                    Department = people.known_for_department,
                    DOB = people.birthday
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
                actorModel.Bio = string.Format("Biography: {0}", (string.IsNullOrEmpty(response.biography) ? "Not available" : response.biography));
                actorModel.Department = string.Format("Known for: {0}", response.known_for_department);
                actorModel.DOB = string.Format("Birthday: {0}", Convert.ToDateTime(response.birthday).ToLongDateString());
                actorModel.POB = string.Format("Place of birth: {0}", response.place_of_birth);
                actorModel.Adult = response.adult;
                actorModel.Gender = string.Format("Gender: {0}", (response.gender.Equals(1) ? "Female" : "Male"));
                actorModel.HomePage = response.homepage;

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
