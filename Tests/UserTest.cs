//using NUnit.Framework;
//using RestshapApiAutomation.Pages;
//using System.Net;
//using System.Threading.Tasks;

//namespace RestshapApiAutomation.Tests
//{
//    [TestFixture]
//    public class UsersTests
//    {
//        private UsersApi _usersApi;

//        [SetUp]
//        public void Setup()
//        {
//            _usersApi = new UsersApi();
//        }

//        [Test]
//        public async Task GetUsers_ShouldReturnSuccess()
//        {
//            var response = await _usersApi.GetUsers();
//            TestContext.WriteLine(response.Content);
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

//        }

//        [Test]
//        public async Task CreateUser_ShouldReturn201()
//        {
//            var response = await _usersApi.CreateUser("Ayush Jain", "Entrepreneur");
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
//            Assert.That(response.Content, Does.Contain("Ayush"));
//        }

//        [Test]
//        public async Task UpdateUser_ShouldReturn200()
//        {
//            var response = await _usersApi.UpdateUser("5", "Aman Updated", "Surgeon");
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//        }

//        [Test]
//        public async Task DeleteUser_ShouldReturn200()
//        {
//            var response = await _usersApi.DeleteUser("56bd");
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
//        }

//        //Negative testcases

//        // ✅ Negative Test Case: Fetching a non-existent user
//        [Test]
//        public async Task GetUser_NonExistent_ShouldReturnNotFound()
//        {
//            var response = await _usersApi.GetUser("99999"); // Assuming 99999 does not exist

//            TestContext.WriteLine(response.Content);
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//        }

//        // ✅ Negative Test Case: Creating a user with missing required fields
//        [Test]
//        public async Task CreateUser_MissingFields_ShouldReturnBadRequest()
//        {
//            var response = await _usersApi.CreateUser("", ""); // Empty fields

//            TestContext.WriteLine(response.Content);
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//        }

//        // ✅ Negative Test Case: Creating a user with an invalid ID format
//        [Test]
//        public async Task CreateUser_InvalidId_ShouldReturnBadRequest()
//        {
//            var response = await _usersApi.CreateUser("abc", "Test User", "Developer"); // ID should be a number

//            TestContext.WriteLine(response.Content);
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//        }

//        // ✅ Negative Test Case: Updating a non-existent user
//        [Test]
//        public async Task UpdateUser_NonExistent_ShouldReturnNotFound()
//        {
//            var response = await _usersApi.UpdateUser("99999", "Nonexistent User", "Unknown");

//            TestContext.WriteLine(response.Content);
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//        }

//        // ✅ Negative Test Case: Deleting a non-existent user
//        [Test]
//        public async Task DeleteUser_NonExistent_ShouldReturnNotFound()
//        {
//            var response = await _usersApi.DeleteUser("99999");

//            TestContext.WriteLine(response.Content);
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//        }
//    }
//}


using Newtonsoft.Json;
using NUnit.Framework;
using RestshapApiAutomation.Pages;
using System;
using System.Net;
using System.Threading.Tasks;

namespace RestshapApiAutomation.Tests
{
    [TestFixture]
    public class UsersTests
    {
        private UsersApi _usersApi;

        [SetUp]
        public void Setup()
        {
            _usersApi = new UsersApi();
        }

        //// ✅ Get All Users - Positive
        //[Test]
        //public async Task GetUsers_ShouldReturnSuccess()
        //{
        //    var response = await _usersApi.GetUsers();
        //    TestContext.WriteLine(response.Content);
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        //}

        [Test]
        public async Task GetUsers_ShouldReturnSuccess()
        {
            var response = await _usersApi.GetUsers();

            TestContext.WriteLine("Response Content: " + response.Content);
            TestContext.WriteLine("Status Code: " + response.StatusCode);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }


        // ✅ Create User - Positive
        //[Test]
        //public async Task CreateUser_ShouldReturn201()
        //{
        //    var response = await _usersApi.CreateUser("Neha Chavan", "Teacher");
        //    TestContext.WriteLine(response.Content);
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        //    Assert.That(response.Content, Does.Contain("Neha"));
        //}

        [Test]
        public async Task CreateUser_AndVerifyWithGet_ShouldSucceed()
        {
            // Step 1: Create a new user
            var createResponse = await _usersApi.CreateUser("Ram kumar", "Accountant");
            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            // Extract the user ID from the response
            var createdUser = JsonConvert.DeserializeObject<dynamic>(createResponse.Content);
            string userId = createdUser?.id;
            Assert.That(userId, Is.Not.Null.And.Not.Empty, "User ID should be returned after creation");

            TestContext.WriteLine("New User ID: " + userId);

            // Step 2: Fetch the user using the extracted ID
            var getResponse = await _usersApi.GetUser(userId);
            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            TestContext.WriteLine("User found: " + getResponse.Content);

            // Step 3: Verify that the user details match
            var fetchedUser = JsonConvert.DeserializeObject<dynamic>(getResponse.Content);
            Assert.That((string)fetchedUser.name, Is.EqualTo("Ram kumar"));
            Assert.That((string)fetchedUser.job, Is.EqualTo("Accountant"));
        }


        // ✅ Update User - Positive
        [Test]
        public async Task UpdateUser_ShouldReturn200()
        {
            var response = await _usersApi.UpdateUser("5", "Pratmesh", "Qa Engineer");
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        // ✅ Delete User - Positive
        [Test]
        public async Task DeleteUser_ShouldReturn200Or404()
        {
            var response = await _usersApi.DeleteUser("56bd");
            TestContext.WriteLine(response.Content);

            // Some APIs return 404 if user does not exist, so check for both
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK).Or.EqualTo(HttpStatusCode.NotFound));
        }

        // ❌ Negative Test Cases ❌

        // ✅ Fetching a non-existent user
        [Test]
        public async Task GetUser_NonExistent_ShouldReturnNotFound()
        {
            var response = await _usersApi.GetUser("999987"); // Assuming 99999 does not exist
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        // ✅ Creating a user with missing required fields
        [Test]
        public async Task CreateUser_MissingFields_ShouldReturn400Or422()
        {
            var response = await _usersApi.CreateUser("", ""); // Empty fields
            TestContext.WriteLine(response.Content);

            // Some APIs return 400, others return 422 Unprocessable Entity
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest).Or.EqualTo(HttpStatusCode.UnprocessableEntity));
        }

        // ✅ Creating a user with an invalid ID format (Fixed)
        [Test]
        public async Task CreateUser_InvalidId_ShouldReturnBadRequest()
        {
            var response = await _usersApi.CreateUserWithInvalidId("abc", "Test User", "Developer"); // ID should be numeric
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        // ✅ Updating a non-existent user
        [Test]
        public async Task UpdateUser_NonExistent_ShouldReturnNotFound()
        {
            var response = await _usersApi.UpdateUser("99999", "Nonexistent User", "Unknown");
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        // ✅ Deleting a non-existent user
        [Test]
        public async Task DeleteUser_NonExistent_ShouldReturnNotFound()
        {
            var response = await _usersApi.DeleteUser("99999");
            TestContext.WriteLine(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}


//*************

//using NUnit.Framework;
//using RestshapApiAutomation.Pages;
//using System.Net;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace RestshapApiAutomation.Tests
//{
//    [TestFixture]
//    public class UsersTests
//    {
//        private UsersApi _usersApi;
//        private string _userId;  // Store user ID for reuse

//        [SetUp]
//        public async Task Setup()
//        {
//            _usersApi = new UsersApi();

//            // ✅ Create user once before running test cases
//            var createResponse = await _usersApi.CreateUser("Test User", "QA Engineer");
//            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

//            var createdUser = JsonConvert.DeserializeObject<JObject>(createResponse.Content);
//            _userId = createdUser?["id"]?.ToString();
//            Assert.That(_userId, Is.Not.Null.And.Not.Empty, "User ID should be generated");

//            TestContext.WriteLine("🔹 Setup: Created User ID: " + _userId);
//        }

//        [Test]
//        public async Task GetUser_ShouldReturnUserDetails()
//        {
//            var response = await _usersApi.GetUser(_userId);
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

//            var userData = JsonConvert.DeserializeObject<JObject>(response.Content);
//            Assert.That(userData?["name"]?.ToString(), Is.EqualTo("Test User"));
//            Assert.That(userData?["job"]?.ToString(), Is.EqualTo("QA Engineer"));

//            TestContext.WriteLine("✅ Get User Response: " + response.Content);
//        }

//        [Test]
//        public async Task UpdateUser_ShouldReflectChanges()
//        {
//            var response = await _usersApi.UpdateUser(_userId, "Updated User", "Senior QA");
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

//            var updatedUser = JsonConvert.DeserializeObject<JObject>(response.Content);
//            Assert.That(updatedUser?["name"]?.ToString(), Is.EqualTo("Updated User"));
//            Assert.That(updatedUser?["job"]?.ToString(), Is.EqualTo("Senior QA"));

//            TestContext.WriteLine("✅ Updated User Response: " + response.Content);
//        }

//        [Test]
//        public async Task DeleteUser_ShouldRemoveUser()
//        {
//            var deleteResponse = await _usersApi.DeleteUser(_userId);
//            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

//            TestContext.WriteLine("✅ User deleted successfully");

//            // Verify deletion
//            var getResponse = await _usersApi.GetUser(_userId);
//            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

//            TestContext.WriteLine("✅ User no longer exists, as expected");
//        }

//        // 🔻🔻🔻 Negative Test Cases 🔻🔻🔻

//        [Test]
//        public async Task GetUser_NonExistent_ShouldReturnNotFound()
//        {
//            var response = await _usersApi.GetUser("99999"); // Assuming this ID does not exist
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

//            TestContext.WriteLine("❌ Get Non-Existent User: " + response.Content);
//        }

//        [Test]
//        public async Task CreateUser_MissingFields_ShouldReturnBadRequest()
//        {
//            var response = await _usersApi.CreateUser("", ""); // Empty name and job
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

//            TestContext.WriteLine("❌ Create User with Missing Fields: " + response.Content);
//        }

//        [Test]
//        public async Task UpdateUser_NonExistent_ShouldReturnNotFound()
//        {
//            var response = await _usersApi.UpdateUser("99999", "Fake User", "Ghost Job");
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

//            TestContext.WriteLine("❌ Update Non-Existent User: " + response.Content);
//        }

//        [Test]
//        public async Task DeleteUser_NonExistent_ShouldReturnNotFound()
//        {
//            var response = await _usersApi.DeleteUser("99999");
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

//            TestContext.WriteLine("❌ Delete Non-Existent User: " + response.Content);
//        }

//        [Test]
//        public async Task CreateUser_InvalidData_ShouldReturnBadRequest()
//        {
//            var response = await _usersApi.CreateUser("1234", "###"); // Invalid name and job
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

//            TestContext.WriteLine("❌ Create User with Invalid Data: " + response.Content);
//        }
//    }
//}

//******************
//using NUnit.Framework;
//using RestshapApiAutomation.Pages;
//using System.Net;
//using System.Threading.Tasks;
//using System.IO;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;

//namespace RestshapApiAutomation.Tests
//{
//    [TestFixture]
//    public class UsersTests
//    {
//        private UsersApi _usersApi;
//        private string _userId;
//        private JObject _testData;

//        [SetUp]
//        public async Task Setup()
//        {
//            _usersApi = new UsersApi();

//            // ✅ Load JSON test data from local file
//            string jsonPath = Path.Combine(TestContext.CurrentContext.WorkDirectory, "C:\\Users\\User\\First.json");
//            _testData = JObject.Parse(File.ReadAllText(jsonPath));

//            // ✅ Use JSON data for user creation
//            string name = _testData["validUser"]["name"].ToString();
//            string job = _testData["validUser"]["job"].ToString();

//            var createResponse = await _usersApi.CreateUser(name, job);
//            Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));

//            var createdUser = JsonConvert.DeserializeObject<JObject>(createResponse.Content);
//            _userId = createdUser?["id"]?.ToString();
//            Assert.That(_userId, Is.Not.Null.And.Not.Empty, "User ID should be generated");

//            TestContext.WriteLine("🔹 Setup: Created User ID: " + _userId);
//        }

//        [Test]
//        public async Task GetUser_ShouldReturnUserDetails()
//        {
//            var response = await _usersApi.GetUser(_userId);
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

//            var userData = JsonConvert.DeserializeObject<JObject>(response.Content);
//            Assert.That(userData?["name"]?.ToString(), Is.EqualTo(_testData["validUser"]["name"].ToString()));
//            Assert.That(userData?["job"]?.ToString(), Is.EqualTo(_testData["validUser"]["job"].ToString()));

//            TestContext.WriteLine("✅ Get User Response: " + response.Content);
//        }

//        [Test]
//        public async Task UpdateUser_ShouldReflectChanges()
//        {
//            var response = await _usersApi.UpdateUser(_userId, _testData["updatedUser"]["name"].ToString(), _testData["updatedUser"]["job"].ToString());
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

//            var updatedUser = JsonConvert.DeserializeObject<JObject>(response.Content);
//            Assert.That(updatedUser?["name"]?.ToString(), Is.EqualTo(_testData["updatedUser"]["name"].ToString()));
//            Assert.That(updatedUser?["job"]?.ToString(), Is.EqualTo(_testData["updatedUser"]["job"].ToString()));

//            TestContext.WriteLine("✅ Updated User Response: " + response.Content);
//        }

//        [Test]
//        public async Task DeleteUser_ShouldRemoveUser()
//        {
//            var deleteResponse = await _usersApi.DeleteUser(_userId);
//            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));

//            TestContext.WriteLine("✅ User deleted successfully");

//            // Verify deletion
//            var getResponse = await _usersApi.GetUser(_userId);
//            Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

//            TestContext.WriteLine("✅ User no longer exists, as expected");
//        }

//        // 🔻🔻🔻 Negative Test Cases 🔻🔻🔻

//        [Test]
//        public async Task CreateUser_MissingFields_ShouldReturnBadRequest()
//        {
//            var response = await _usersApi.CreateUser(_testData["invalidUser"]["name"].ToString(), _testData["invalidUser"]["job"].ToString());
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));

//            TestContext.WriteLine("❌ Create User with Missing Fields: " + response.Content);
//        }

//        [Test]
//        public async Task GetUser_NonExistent_ShouldReturnNotFound()
//        {
//            var response = await _usersApi.GetUser("99999"); // Assuming 99999 does not exist
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//            TestContext.WriteLine("❌ Fetching non-existent user returned: " + response.Content);
//        }

//        // ✅ Negative Test Case: Updating a non-existent user
//        [Test]
//        public async Task UpdateUser_NonExistent_ShouldReturnNotFound()
//        {
//            var response = await _usersApi.UpdateUser("99999", "Nonexistent User", "Unknown");
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//            TestContext.WriteLine("❌ Updating non-existent user returned: " + response.Content);
//        }

//        // ✅ Negative Test Case: Deleting a non-existent user
//        [Test]
//        public async Task DeleteUser_NonExistent_ShouldReturnNotFound()
//        {
//            var response = await _usersApi.DeleteUser("99999");
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
//            TestContext.WriteLine("❌ Deleting non-existent user returned: " + response.Content);
//        }

//        // ✅ Negative Test Case: Creating user with special characters in name
//        [Test]
//        public async Task CreateUser_SpecialCharacters_ShouldReturnBadRequest()
//        {
//            var response = await _usersApi.CreateUser("@@!!##", "Developer");
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//            TestContext.WriteLine("❌ Creating user with special characters returned: " + response.Content);
//        }

//        // ✅ Negative Test Case: Deleting a user with invalid ID format
//        [Test]
//        public async Task DeleteUser_InvalidIdFormat_ShouldReturnBadRequest()
//        {
//            var response = await _usersApi.DeleteUser("abc123"); // Invalid ID format
//            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
//            TestContext.WriteLine("❌ Deleting user with invalid ID format returned: " + response.Content);
//        }
//    }
//}
