using AllInOne.Common;
using AllInOne.Common.Authentication;
using AllInOne.Common.Exceptions;
using AllInOne.Common.Session;
using AllInOne.Domains.Core.Identity;
using AllInOne.Domains.Core.Identity.Entities;
using AllInOne.Servers.API.Controllers.Identity.Dtos;
using AllInOne.Servers.API.Filters.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace AllInOne.Servers.API.Controllers.Identity
{
    [Route(Constants.Api.V1.Authentication.Url)]
    [ApiController]
    public class AuthenticationController : AuthentifiedBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(
          IAuthenticationService authenticationService,
          IUserManager userManager,
          IUserSession session,
          IMapper mapper,
          ILogger<AuthenticationController> logger
        ) : base(session, userManager, mapper, logger)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [Route(Constants.Api.V1.Authentication.Register)]
        public async Task<IActionResult> RegisterUserAsync([FromBody]RegistrationRequestDto dto)
        {
            Logger.LogInformation($"{nameof(RegisterUserAsync)}", dto.Email, dto.Firstname, dto.Lastname);
            var userToRegister = new User(
              dto.Email,
              dto.Firstname,
              dto.Lastname
            );

            await _userManager.RegisterAsync(userToRegister, dto.Password);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(ApiErrorDto), (int)HttpStatusCode.Unauthorized)]
        [Route(Constants.Api.V1.Authentication.Login)]
        public async Task<IActionResult> LoginUserAsync([FromBody]LoginRequestDto dto)
        {
            Logger.LogInformation($"{nameof(LoginUserAsync)}", dto.Email);
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                Logger.LogWarning($"{nameof(LoginUserAsync)}, User not found", dto.Email, user);
                throw new LocalException("Unauthorized", HttpStatusCode.Unauthorized);
            }
            else if (user.LockoutEnabled){
                Logger.LogWarning($"{nameof(LoginUserAsync)}, User locked", dto.Email, user);
                throw new LocalException("Accound locked", HttpStatusCode.Unauthorized);
            }
            else if (!await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                Logger.LogWarning($"{nameof(LoginUserAsync)}, Login failed", dto.Email, user);
                throw new LocalException("Unauthorized", HttpStatusCode.Unauthorized);
            }

            return new ObjectResult(new LoginResponseDto
            {
                CurrentUser = Mapper.Map<User, UserDto>(user),
                Token = _authenticationService.GenerateToken(user)
            });
        }
    }
}
