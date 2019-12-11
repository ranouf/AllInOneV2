using AllInOne.Common;
using AllInOne.Common.Paging;
using AllInOne.Common.Session;
using AllInOne.Domains.Core.Identity;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Servers.API.Attributes;
using AllInOne.Servers.API.Controllers.Dtos.Paging;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using AllInOne.Servers.API.Filters.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AllInOne.Servers.API.Controllers.Identity
{
    [Route(Constants.Api.V1.User.Url)]
    [AuthorizeAdministrators]
    public class UserController : AuthentifiedBaseController
    {
        private readonly IRoleManager _roleManager;

        public UserController(
            IUserSession session,
            IUserManager userManager,
            IRoleManager roleManager,
            ILogger<UserController> logger,
            IMapper mapper
        ) : base(session, userManager, mapper, logger)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [AuthorizeAdministratorAndManagers]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute]Guid id)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(GetUserByIdAsync)}", currentUser, id);
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Logger.LogWarning($"{nameof(GetUserByIdAsync)}, User not found", currentUser, id);
                return NotFound();
            }
            return new ObjectResult(Mapper.Map<User, UserDto>(user));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDto<UserDto, Guid?>), 200)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [AuthorizeAdministrators]
        public async Task<IActionResult> GetUsersAsync([FromQuery]PagedRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(GetUsersAsync)}", currentUser, dto);
            var result = await _userManager.GetAllAsync(
                dto.Filter,
                dto.MaxResultCount,
                dto.SkipCount);

            return new ObjectResult(Mapper.Map<PagedResult<User>, PagedResultDto<UserDto, Guid?>>(result));
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [AuthorizeAdministrators]
        public async Task<IActionResult> CreateUserAsync([FromBody]CreateUserRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(CreateUserAsync)}", currentUser, dto);
            var role = await _roleManager.FindByIdAsync(dto.RoleId);
            if (role == null)
            {
                Logger.LogWarning($"{nameof(CreateUserAsync)}, Role not found", currentUser, dto);
                return NotFound();
            }

            var newUser = new User(
              dto.Email,
              dto.Firstname,
              dto.Lastname
            );

            newUser = await _userManager.CreateAsync(newUser, dto.Password, role);
            return new ObjectResult(Mapper.Map<User, UserDto>(newUser));
        }

        [HttpPut]
        [ProducesResponseType(typeof(UserDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [AuthorizeAdministrators]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateUserAsync([FromRoute]Guid id, [FromBody]UpdateUserRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(UpdateUserAsync)}", currentUser, dto);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Logger.LogWarning($"{nameof(UpdateUserAsync)}, User not found", currentUser, dto);
                return NotFound("User not found");
            }

            var role = await _roleManager.FindByIdAsync(dto.RoleId);
            if (role == null)
            {
                Logger.LogWarning($"{nameof(UpdateUserAsync)}, Role not found", currentUser, dto);
                return NotFound("Role not found");
            }

            user.Update(
                dto.Firstname,
                dto.Lastname
            );
            user.SetRole(role);

            user = await _userManager.UpdateAsync(user);
            return new ObjectResult(Mapper.Map<User, UserDto>(user));
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute]Guid id)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(AllowToLoginAsync)}", currentUser, id);
            if (currentUser.Id == id)
            {
                Logger.LogWarning($"{nameof(AllowToLoginAsync)}, Can't delete himself", currentUser, id);
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Logger.LogWarning($"{nameof(AllowToLoginAsync)}, User not found", currentUser, id);
                return NotFound();
            }

            await _userManager.DeleteAsync(user);

            return Ok();
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Route(Constants.Api.V1.User.Lock)]
        public async Task<IActionResult> LockUserAsync([FromRoute]Guid id)
        {
            return await AllowToLoginAsync(id, false);
        }

        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Route(Constants.Api.V1.User.Unlock)]
        public async Task<IActionResult> UnlockUserAsync([FromRoute]Guid id)
        {
            return await AllowToLoginAsync(id, true);
        }

        #region Private

        private async Task<IActionResult> AllowToLoginAsync(Guid id, bool allow)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation(nameof(AllowToLoginAsync), currentUser, id, allow);
            if (currentUser.Id == id)
            {
                Logger.LogWarning($"{nameof(AllowToLoginAsync)}, Can't allow himself", currentUser, id);
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Logger.LogWarning($"{nameof(AllowToLoginAsync)}, User not found", currentUser, id);
                return NotFound();
            }

            await _userManager.AllowUserToLoginAsync(user, allow);

            return Ok();
        }

        #endregion
    }
}
