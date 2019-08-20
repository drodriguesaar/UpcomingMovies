using UpcomingMovies.Consts;

namespace UpcomingMovies.Infra
{
    public static class StringExtensions
    {
        public static string BuildImageURI(this string posterPath, string size = "200")
        {
            if (string.IsNullOrEmpty(posterPath))
            {
                return ResourceConsts.IMG_URL_NOT_SET;
            }
            return string.Format(ResourceConsts.IMG_URL, posterPath, size);
        }
    }
}
