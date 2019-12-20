using AllInOne.Integration.Tests.Data;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests.Extensions
{
    public static class TestServerFixtureExtensions
    {
        public static async Task<HttpResponseMessage> AuthenticateAsAdministratorAsync(this TestServerFixture testServerFixture, ITestOutputHelper output)
        {
            return await testServerFixture.AuthenticateAsync(
                TestUserDataBuilder.AdministratorEmail,
                TestUserDataBuilder.Password,
                output
            );
        }

        public static async Task<HttpResponseMessage> AuthenticateAsManagerAsync(this TestServerFixture testServerFixture, ITestOutputHelper output)
        {
            return await testServerFixture.AuthenticateAsync(
                TestUserDataBuilder.ManagerEmail,
                TestUserDataBuilder.Password,
                output
            );
        }

        public static async Task<HttpResponseMessage> AuthenticateAsUserAsync(this TestServerFixture testServerFixture, ITestOutputHelper output)
        {
            return await testServerFixture.AuthenticateAsync(
                TestUserDataBuilder.UserEmail,
                TestUserDataBuilder.Password,
                output
            );
        }

        public static async Task<HttpResponseMessage> AuthenticateAsAsync(this TestServerFixture testServerFixture, string email, string password, ITestOutputHelper output)
        {
            return await testServerFixture.AuthenticateAsync(
                email,
                password,
                output
            );
        }

        public static void AuthenticateAsAnonymous(this TestServerFixture testServerFixture)
        {
            testServerFixture.Client.DefaultRequestHeaders.Authorization = null;
        }

        #region Private

        private static async Task<HttpResponseMessage> AuthenticateAsync(this TestServerFixture testServerFixture, string email, string password, ITestOutputHelper output)
        {
            var requestData = new LoginRequestDto { Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            output.WriteLine($"METHOD POST, url:'{AllInOne.Common.Constants.Api.V1.Authentication.Login}' dto:'{requestData.ToJson()}'");
            var response = await testServerFixture.Client.PostAsync(AllInOne.Common.Constants.Api.V1.Authentication.Login, content);

            var dto = await response.ConvertToAsync<LoginResponseDto>(output);
            testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", dto.Token);
            return response;
        }
        #endregion
    }
}
