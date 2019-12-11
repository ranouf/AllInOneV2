using AllInOne.Integration.Tests.Data;
using AllInOne.Integration.Tests.Extensions;
using AllInOne.Servers.API.Controllers.Dtos.Paging;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static AllInOne.Common.Constants;

namespace AllInOne.Integration.Tests.Controllers.Identity
{
    [Collection(Constants.TEST_COLLECTION)]
    public class Users_Tests : BaseTest
    {
        private const string Email = "newuser@sidekickinteractive.com";
        private const string Password = "Password123#";

        public Users_Tests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Should_Not_Get_Users_As_Anonymous()
        {
            TestServerFixture.AuthenticateAsAnonymous();
            var response = await TestServerFixture.Client.GetAsync(
                AllInOne.Common.Constants.Api.V1.User.Url,
                Output,
                new PagedRequestDto
                {
                    MaxResultCount = 2,
                    SkipCount = 0
                }
            );
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Should_Get_Two_Users_Order_By_LastName_As_Administrator()
        {
            await TestServerFixture.AuthenticateAsAdministratorAsync(Output);
            var response = await TestServerFixture.Client.GetAsync(
                AllInOne.Common.Constants.Api.V1.User.Url,
                Output,
                new PagedRequestDto
                {
                    MaxResultCount = 2,
                    SkipCount = 0
                }
            );
            var dto = await response.ConvertToAsync<PagedResultDto<UserDto, Guid?>>(Output);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(dto);
            Assert.Equal(3, dto.TotalCount);
            Assert.Equal(2, dto.Items.Count);
            Assert.True(dto.HasNext);
        }

        [Fact]
        public async Task Should_Not_Lock_Myself_As_Administrator()
        {
            // As Admin
            var dto = await TestServerFixture.AuthenticateAsAdministratorAsync(Output);

            // Not Lock
            var response = await TestServerFixture.Client.PutByIdAsync(
                AllInOne.Common.Constants.Api.V1.User.Lock,
                Output,
                dto.CurrentUser
            );
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Should_Lock_And_Unlock_User_As_Administrator()
        {
            // As User
            var dto = await TestServerFixture.AuthenticateAsUserAsync(Output);

            // As Admin
            await TestServerFixture.AuthenticateAsAdministratorAsync(Output);

            // Lock User
            var response = await TestServerFixture.Client.PutByIdAsync(
                AllInOne.Common.Constants.Api.V1.User.Lock,
                Output,
                dto.CurrentUser
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // As locked user
            response = await TestServerFixture.AuthenticateAsAsync(
                TestUserDataBuilder.UserEmail,
                TestUserDataBuilder.Password,
                Output
            );
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

            // As Admin
            await TestServerFixture.AuthenticateAsAdministratorAsync(Output);

            // Unlock User
            response = await TestServerFixture.Client.PutByIdAsync(
                AllInOne.Common.Constants.Api.V1.User.Unlock,
                Output,
                dto.CurrentUser
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // As User
            response = await TestServerFixture.AuthenticateAsAsync(
                TestUserDataBuilder.UserEmail,
                TestUserDataBuilder.Password,
                Output
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Should_Not_Delete_Myself_As_Administrator()
        {
            // As Admin
            var dto = await TestServerFixture.AuthenticateAsAdministratorAsync(Output);

            // Not Lock
            var response = await TestServerFixture.Client.DeleteAsync(
                AllInOne.Common.Constants.Api.V1.User.Url,
                Output,
                dto.CurrentUser
            );
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }


        [Fact]
        public async Task Should_Create_Login_And_Delete_As_Administrator()
        {
            // As Admin
            await TestServerFixture.AuthenticateAsAdministratorAsync(Output);

            // Roles
            var response = await TestServerFixture.Client.GetAsync(
                AllInOne.Common.Constants.Api.V1.Role.Url,
                Output
            );
            var rolesDto = await response.ConvertToAsync<RoleDto[]>(Output);

            // Create new User
            var newUser = new CreateUserRequestDto
            {
                Email = Email,
                Firstname = "FirstName",
                Lastname = "LastName",
                Password = Password,
                PasswordConfirmation = Password,
                RoleId = rolesDto.First(r => r.Name == Domains.Core.Constants.Roles.Administrator).Id
            };
            response = await TestServerFixture.Client.PostAsync(
                AllInOne.Common.Constants.Api.V1.User.Url,
                Output,
                newUser
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Login
            response = await TestServerFixture.AuthenticateAsAsync(Email, Password, Output);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dto = await response.ConvertToAsync<LoginResponseDto>(Output);

            // As Admin
            await TestServerFixture.AuthenticateAsAdministratorAsync(Output);

            // Delete User
            response = await TestServerFixture.Client.DeleteAsync(
                AllInOne.Common.Constants.Api.V1.User.Url,
                Output,
                dto.CurrentUser
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Not Login
            response = await TestServerFixture.AuthenticateAsAsync(Email, Password, Output);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
