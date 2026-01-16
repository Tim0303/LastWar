using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Mvc;
using NETWebTemp.Service.Interfaces;
using NETWebTemp.Service.ViewModels;
using System.Security.Claims;

namespace NETWebTemp.Api.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]
        [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> Login([FromBody] LoginVm userModel)
        {
            var result = await _authenticateService.Login(userModel);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("登入失敗");
            }
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<object>(StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> LogOut()
        {
            var userId = new Guid(User.Claims.FirstOrDefault(x => x.ValueType == ClaimTypes.NameIdentifier).Value);

            await _authenticateService.LogOut(userId);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType<object>(StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> Register([FromBody] RegisterVm model)
        {
            var result = await _authenticateService.Register(model);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// RefreshToken
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK, "application/json")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenVm model)
        {
            var result = await _authenticateService.RefreshToken(model);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}