using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace UpcomingMovies.Service
{
    internal class ServiceBase
    {

        protected HttpClient _baseHttpClient { get; private set; }
        protected string EndPointDomain { get; private set; }
        protected string EndPointAPIKey { get; private set; }
        protected string EndPoint { get; set; }

        public ServiceBase(string endPointDomain, string endPointAPIKey)
        {
            this.EndPointAPIKey = endPointAPIKey;
            this.EndPointDomain = endPointDomain;
            SetBaseHttpClient();
        }

        void SetBaseHttpClient()
        {
            var httpClientHandler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
            };
            _baseHttpClient = new HttpClient(httpClientHandler, false)
            {
                BaseAddress = new Uri(EndPointDomain),

            };

            _baseHttpClient.DefaultRequestHeaders.Clear();
            _baseHttpClient.DefaultRequestHeaders.Accept.Clear();
            _baseHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected async Task<TResult> Get<TData, TResult>(TData data)
        {
            var queryString = GenerateQueryString(data);
            var resposta = await _baseHttpClient.GetAsync(string.Format("{0}&{1}", EndPoint, queryString));
            resposta.EnsureSuccessStatusCode();
            var respostastring = await resposta.Content.ReadAsStringAsync();
            var returnData = Deserialize<TResult>(respostastring);
            return returnData;
        }

        private string Serialize<T>(T data)
        {
            var postData = JsonConvert.SerializeObject(data, null, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return postData;
        }

        private T Deserialize<T>(string data)
        {
            var returnData = JsonConvert.DeserializeObject<T>(data, new JsonSerializerSettings { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore });
            return returnData;
        }

        private string GenerateQueryString(object param)
        {
            var paramType = param.GetType();
            var paramTypeProperties = paramType.GetProperties();
            var querystring = string.Empty;
            var result = new Dictionary<string, object>();
            result = ExtractProperties(param, result);
            result = result.Where(r => r.Value != null).ToList().ToDictionary(x => x.Key, x => x.Value);
            querystring = BuildQueryString(result);
            return querystring;
        }

        private string BuildQueryString(Dictionary<string, object> parameterCollection)
        {
            var keyValueStrings = parameterCollection.Select(pair => string.Format("{0}={1}", pair.Key, pair.Value));
            return string.Join("&", keyValueStrings).ToLower();
        }

        private Dictionary<string, object> ExtractProperties(object param, Dictionary<string, object> result)
        {
            result = param.GetType()
                          .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                          .ToDictionary(prop => prop.Name, prop => prop.GetValue(param, null));
            return result;
        }
    }
}
