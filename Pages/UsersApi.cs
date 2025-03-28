//using RestSharp;
//using System.Threading.Tasks;

//namespace RestshapApiAutomation.Pages
//{
//    public class UsersApi : BaseApi
//    {
//        public async Task<RestResponse> GetUsers()
//        {
//            var request = new RestRequest("/users", Method.Get);
//            return await ExecuteRequest(request);
//        }

//        public async Task<RestResponse> CreateUser( string name, string job)
//        {
//            var request = new RestRequest("/users", Method.Post);
//            request.AddJsonBody(new { name, job });
//            return await ExecuteRequest(request);
//        }

//        public async Task<RestResponse> UpdateUser(string id, string name, string job)
//        {
//            var request = new RestRequest($"/users/{id}", Method.Put);
//            request.AddJsonBody(new { name, job });
//            return await ExecuteRequest(request);
//        }

//        public async Task<RestResponse> DeleteUser(string id)
//        {
//            var request = new RestRequest($"/users/{id}", Method.Delete);
//            return await ExecuteRequest(request);
//        }
//    }
//}


using RestSharp;
using System.Net;
using System.Threading.Tasks;

namespace RestshapApiAutomation.Pages
{
    public class UsersApi : BaseApi
    {
        // ✅ Get all users
        public async Task<RestResponse> GetUsers()
        {
            var request = new RestRequest("/users", Method.Get);
            return await ExecuteRequest(request);
        }

        // ✅ Get a specific user (Handles non-existent users)
        public async Task<RestResponse> GetUser(string userId)
        {
            var request = new RestRequest($"/users/{userId}", Method.Get);
            var response = await ExecuteRequest(request);

            // Return response as-is, test cases will handle assertions
            return response;
        }

        // ✅ Create a user (Handles missing fields)
        public async Task<RestResponse> CreateUser(string name, string job)
        {
            var request = new RestRequest("/users", Method.Post);

            // If name or job is missing, send empty JSON
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(job))
            {
                request.AddJsonBody(new { });
            }
            else
            {
                request.AddJsonBody(new { name, job });
            }

            return await ExecuteRequest(request);
        }

        // ✅ Update a user
        public async Task<RestResponse> UpdateUser(string id, string name, string job)
        {
            var request = new RestRequest($"/users/{id}", Method.Put);
            request.AddJsonBody(new { name, job });
            return await ExecuteRequest(request);
        }

        // ✅ Delete a user
        public async Task<RestResponse> DeleteUser(string id)
        {
            var request = new RestRequest($"/users/{id}", Method.Delete);
            return await ExecuteRequest(request);
        }

        // ✅ Create User with Invalid ID Format (Handles invalid inputs)
        public async Task<RestResponse> CreateUserWithInvalidId(string id, string name, string job)
        {
            var request = new RestRequest("/users", Method.Post);
            request.AddJsonBody(new { id, name, job });

            return await ExecuteRequest(request);
        }
    }
}
