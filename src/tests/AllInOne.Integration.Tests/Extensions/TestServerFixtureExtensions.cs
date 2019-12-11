using AllInOne.Servers.API.Controllers.Identity.Dtos;
using AllInOne.Integration.Tests.Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using static AllInOne.Common.Constants;

namespace AllInOne.Integration.Tests.Extensions
{
    public static class TestServerFixtureExtensions
    {
        public static async Task<LoginResponseDto> AuthenticateAsAdministratorAsync(this TestServerFixture testServerFixture, ITestOutputHelper output)
        {
            return await testServerFixture.AuthenticateAsync(
                TestUserDataBuilder.AdministratorEmail,
                TestUserDataBuilder.Password,
                output
            );
        }

        public static async Task<LoginResponseDto> AuthenticateAsManagerAsync(this TestServerFixture testServerFixture, ITestOutputHelper output)
        {
            return await testServerFixture.AuthenticateAsync(
                TestUserDataBuilder.ManagerEmail,
                TestUserDataBuilder.Password,
                output
            );
        }

        public static async Task<LoginResponseDto> AuthenticateAsUserAsync(this TestServerFixture testServerFixture, ITestOutputHelper output)
        {
            return await testServerFixture.AuthenticateAsync(
                TestUserDataBuilder.UserEmail,
                TestUserDataBuilder.Password,
                output
            );
        }

        public static Task<HttpResponseMessage> AuthenticateAsAsync(this TestServerFixture testServerFixture, string email, string password, ITestOutputHelper output)
        {
            var requestData = new LoginRequestDto { Email = email, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
            output.WriteLine($"METHOD POST, url:'{Api.V1.Authentication.Login}' dto:'{requestData.ToJson()}'");
            return testServerFixture.Client.PostAsync(Api.V1.Authentication.Login, content);
        }

        public static void AuthenticateAsAnonymous(this TestServerFixture testServerFixture)
        {
            testServerFixture.Client.DefaultRequestHeaders.Authorization = null;
        }

        #region Private
        private static async Task<LoginResponseDto> AuthenticateAsync(this TestServerFixture testServerFixture, string email, string password, ITestOutputHelper output)
        {
            var response = await testServerFixture.AuthenticateAsAsync(email, password, output);
            var dto = await response.ConvertToAsync<LoginResponseDto>(output);
            testServerFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", dto.Token);
            return dto;
        }
        #endregion
    }
}
