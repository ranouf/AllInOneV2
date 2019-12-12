using AllInOne.Integration.Tests.Extensions;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AllInOne.Integration.Tests.Controllers.Identity
{
    [Collection(Constants.TEST_COLLECTION)]
    public class Role_Tests : BaseTest
    {

        public Role_Tests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Should_Get_Roles_As_Administrator()
        {
            await TestServerFixture.AuthenticateAsAdministratorAsync(Output);
            var response = await TestServerFixture.Client.GetAsync(
                Common.Constants.Api.V1.Role.Url,
                Output
            );
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var dto = await response.ConvertToAsync<IEnumerable<RoleDto>>(Output);
            Assert.Equal(3, dto.Count());
        }
    }
}
