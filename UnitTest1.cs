using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Threading.Tasks;

namespace RestshapApi_automation
{
    [TestFixture]
    public class Tests
    {
        private RestClient _client;
        //ashu
        [SetUp]
        public void Setup()
        {
            _client = new RestClient("http://localhost:3100");
        }

        [TearDown]
        public void Teardown()
        {
            _client?.Dispose();
        }


        [Test]
        public async Task Get_ShouldReturnSuccess()
        {
            var request = new RestRequest("/users", Method.Get);

            var response = await _client.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Expected HTTP 200 OK");
            Assert.That(response.Content, Does.Contain("\"id\": \"1\""), "Response should contain user ID 1");
            Assert.That(response.Content, Does.Contain("\"name\": \"John Doe\""), "Response should contain user 'John Doe'");
        }

        [Test]
        public async Task Post_ShouldReturn201()
        {
            var request = new RestRequest("/users", Method.Post);

            // Correct JSON structure
            request.AddJsonBody(new
            {
                id = "5",
                name = "Aman",
                job = "Doctor"
            });

            var response = await _client.ExecuteAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created), "Expected HTTP 201 Created");
                Assert.That(response.Content, Does.Contain("\"name\": \"Aman\""), "Response should contain 'Aman'");
            });
        }

        [Test]
        public async Task Update_ShouldReturn200()
        {
            var request = new RestRequest("/users/5", Method.Put);
            request.AddJsonBody(new
            {
                id = "5",
                name = "Saurabh",
                job = "Administration"
            });

            var response = await _client.ExecuteAsync(request);
            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
            Assert.That(response.Content, Does.Contain("\"name\": \"Saurabh\""));
        }

        [Test]
        public async Task DeletePost_ShouldReturn200()
        {
            var request = new RestRequest("/users/3", Method.Delete);
            var response = await _client.ExecuteAsync(request);

            Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        }

    }
}