using AllInOne.Common;
using AllInOne.Common.Extensions;
using AllInOne.Common.Logging;
using AllInOne.Common.Session;
using AllInOne.Domains.Core.Identity;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using AllInOne.Servers.API.Filters.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace AllInOne.Servers.API.Controllers.Identity
{
    [Route(Constants.Api.V1.Account.Url)]
    [Authorize]
    [ApiController]
    public class AccountController : AuthentifiedBaseController
    {
        public AccountController(
          IUserManager userManager,
          IMapper mapper,
          IUserSession session,
          ILoggerService<AccountController> logger
        ) : base(session, userManager, mapper, logger)
        { }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Authorize]
        [Route(Constants.Api.V1.Account.Password)]
        public async Task<IActionResult> ChangePaswordAsync([FromBody]ChangePasswordRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(ChangePaswordAsync)}, current:{currentUser.ToJson()}, dto: {dto.ToJson()}");
            await _userManager.ChangePasswordAsync(await GetCurrentUserAsync(), dto.CurrentPassword, dto.NewPassword);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Authorize]
        [Route(Constants.Api.V1.Account.Profile)]
        public async Task<IActionResult> UpdateProfileAsync([FromBody]ChangeProfileRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(UpdateProfileAsync)}, current:{currentUser.ToJson()}, dto: {dto.ToJson()}");
            var result = await GetCurrentUserAsync();
            result.Update(dto.Firstname, dto.Lastname);
            result = await _userManager.UpdateAsync(result);
            return new ObjectResult(Mapper.Map<User, UserDto>(result));
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Authorize]
        [Route(Constants.Api.V1.Account.Profile)]
        public async Task<IActionResult> GetProfileAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(GetProfileAsync)}, current:{currentUser.ToJson()}");
            var result = await _userManager.FindByIdAsync(currentUser.Id);
            return new ObjectResult(Mapper.Map<User, UserDto>(result));
        }
    }
}
