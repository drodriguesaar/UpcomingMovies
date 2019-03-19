using System.Diagnostics;

namespace UpcomingMovies.Service
{
    public static class ServiceFactory
    {
        public static IService GetService()
        {
            if (Debugger.IsAttached)
            {
                return new MockService();
            }
            return new ResourceService();
        }
    }
}
