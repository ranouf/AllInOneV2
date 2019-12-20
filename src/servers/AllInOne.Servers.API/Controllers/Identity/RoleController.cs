using AllInOne.Common;
using AllInOne.Common.Extensions;
using AllInOne.Common.Logging;
using AllInOne.Common.Session;
using AllInOne.Domains.Core.Identity;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Servers.API.Attributes;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using AllInOne.Servers.API.Filters.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace AllInOne.Servers.API.Controllers.Identity
{
    [Route(Constants.Api.V1.Role.Url)]
    [AuthorizeAdministrators]
    public class RoleController : AuthentifiedBaseController
    {
        private readonly IRoleManager _roleManager;

        public RoleController(
            IUserSession session,
            IUserManager userManager,
            IRoleManager roleManager,
            ILoggerService<UserController> logger,
            IMapper mapper
        ) : base(session, userManager, mapper, logger)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoleDto>), 200)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(GetAllRolesAsync)}, currentUser:{currentUser.ToJson()}");
            var result = await _roleManager.GetAllAsync();
            return new ObjectResult(Mapper.Map<IEnumerable<Role>, IEnumerable<RoleDto>>(result));
        }
    }
}
