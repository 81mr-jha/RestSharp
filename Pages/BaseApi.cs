using RestSharp;
using System.Threading.Tasks;

namespace RestshapApiAutomation.Pages
{
    public class BaseApi
    {
        protected RestClient _client;

        public BaseApi()
        {
            _client = new RestClient("http://localhost:3100");
        }

        public async Task<RestResponse> ExecuteRequest(RestRequest request)
        {
            return await _client.ExecuteAsync(request);
        }
    }
}
