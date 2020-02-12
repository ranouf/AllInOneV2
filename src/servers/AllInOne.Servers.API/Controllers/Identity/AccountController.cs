using AllInOne.Common;
using AllInOne.Common.Extensions;
using AllInOne.Common.Logging;
using AllInOne.Common.Session;
using AllInOne.Common.Storage;
using AllInOne.Domains.Core.Identity;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using AllInOne.Servers.API.Filters.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AllInOne.Servers.API.Controllers.Identity
{
    [Route(Constants.Api.V1.Account.Url)]
    [Authorize]
    [ApiController]
    public class AccountController : AuthentifiedBaseController
    {
        private readonly IStorageService _storageService;

        public AccountController(
          IUserManager userManager,
          IStorageService storageService,
          IMapper mapper,
          IUserSession session,
          ILoggerService<AccountController> logger
        ) : base(session, userManager, mapper, logger)
        {
            _storageService = storageService;
        }

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
        public async Task<IActionResult> UpdateProfileAsync([FromForm]ChangeProfileRequestDto dto)
        {
            var currentUser = await GetCurrentUserAsync();
            Logger.LogInformation($"{nameof(UpdateProfileAsync)}, current:{currentUser.ToJson()}, dto: {dto.ToJson()}");

            if (dto.ProfileImage != null)
            {
                using (var stream = dto.ProfileImage.OpenReadStream())
                {
                    if (!string.IsNullOrEmpty(currentUser.ProfileImageUrl))
                    {
                        var oldProfileImageUri = new Uri(currentUser.ProfileImageUrl);

                        await _storageService.RemoveFileAsync(Path.GetFileName(oldProfileImageUri.LocalPath));
                    }
                    var fileName = $"{currentUser.Id}-{dto.ProfileImage.FileName}";
                    var newProfileImageUri = await _storageService.SaveFileAsync(stream, fileName);
                    currentUser.SetProfileImageUrl(newProfileImageUri);
                }
            }
            currentUser.Update(dto.Firstname, dto.Lastname);
            currentUser = await _userManager.UpdateAsync(currentUser);
            return new ObjectResult(Mapper.Map<User, UserDto>(currentUser));
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
