using System.Threading.Tasks;
using UpcomingMovies.Consts;
using UpcomingMovies.Enums;

namespace UpcomingMovies.Service
{
    internal class ResourceService : ServiceBase, IService
    {
        public ResourceService() 
            : base(MoviesApiResourcesConsts.RESOURCE, MoviesApiResourcesConsts.APIKEY)
        {
        }
        public async Task<TResult> Consume<TData, TResult>(TData data, string resource, HTTPMethodEnum httpMethodEnum)
        {
            EndPoint = string.Format("{0}/{1}?api_key={2}&language=en-US", EndPointDomain, resource, EndPointAPIKey);
            switch (httpMethodEnum)
            {
                case HTTPMethodEnum.GET:
                    return await Get<TData, TResult>(data);
                default:
                    return default(TResult);
            }
        }
    }
}
