using AllInOne.Integration.Tests.Extensions;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static AllInOne.Common.Constants;

namespace AllInOne.Integration.Tests.Controllers.Identity
{
    [Collection(Constants.TEST_COLLECTION)]
    public class Authentication_Tests : BaseTest
    {
        private const string NewEmail = "newregistration@sidekickinteractive.com";
        private const string NewFirstname = "Firstname";
        private const string NewLastname = "Lastname";
        private const string NewPassword = "Password123#";

        public Authentication_Tests(ITestOutputHelper output): base(output)
        {
        }

        [Fact]
        public async Task Should_Register_As_Anonymous()
        {
            // As Anonymous
            TestServerFixture.AuthenticateAsAnonymous();

            // Register 
            var response = await TestServerFixture.Client.PostAsync(
                Api.V1.Authentication.Register,
                Output,
                new RegistrationRequestDto
                {
                    Email = NewEmail,
                    Firstname = NewFirstname,
                    Lastname = NewLastname,
                    Password = NewPassword,
                    PasswordConfirmation = NewPassword
                }
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response = await TestServerFixture.AuthenticateAsAsync(NewEmail, NewPassword, Output);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dto = await response.ConvertToAsync<LoginResponseDto>(Output);
            Assert.Equal(NewEmail, dto.CurrentUser.Email);
            Assert.Equal(NewFirstname, dto.CurrentUser.Firstname);
            Assert.Equal(NewLastname, dto.CurrentUser.Lastname);

            // As Admin
            await TestServerFixture.AuthenticateAsAdministratorAsync(Output);

            // Delete User
            response = await TestServerFixture.Client.DeleteAsync(
                Api.V1.User.Url,
                Output,
                dto.CurrentUser
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
