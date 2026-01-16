using EFPowerTools.Models.dbo;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NETWebTemp.Service.Interfaces;
using NETWebTemp.Service.ViewModels;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace NETWebTemp.Service.Services
{
    /// <summary>
    ///
    /// </summary>
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IOptionsMonitor<BearerTokenOptions> _bearerTokenOptions;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TimeProvider _timeProvider;

        /// <summary>
        /// 建構式
        /// </summary>
        public AuthenticateService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
            SignInManager<ApplicationUser> signInManager,
            TimeProvider timeProvider
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _bearerTokenOptions = bearerTokenOptions;
            _signInManager = signInManager;
            _timeProvider = timeProvider;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<object> Login(LoginVm userModel)
        {
            var user = await _userManager.FindByNameAsync(userModel.Account);

            if (user == null)
            {
                return null;
            }

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, userModel.Secret, true);

            if (signInResult.Succeeded)
            {
                // _signInManager 自動產製 token
                _signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
                await _signInManager.PasswordSignInAsync(user, userModel.Secret, false, true);
                return new
                {
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<object> Register(RegisterVm model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);

            if (userExists != null)
            {
                return null;
            }

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Account,
                JobTitle = "",
                Name = "",
                Remark = "",
            };
            var result = await _userManager.CreateAsync(user, model.Secret);

            if (!result.Succeeded)
            {
                return null;
            };

            return true;
        }

        /// <summary>
        /// RefreshToken
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<object> RefreshToken(RefreshTokenVm model)
        {
            if (model == null || string.IsNullOrEmpty(model.RefreshToken))
            {
                return false;
            }

            var refreshTokenProtector = _bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
            var refreshTicket = refreshTokenProtector.Unprotect(model.RefreshToken);

            // 驗證 Refresh Token 是否過期或無效
            if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                _timeProvider.GetUtcNow() >= expiresUtc ||
                await _signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not ApplicationUser user)
            {
                return false;
            }
            //更新Token
            _signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;
            await _signInManager.SignInAsync(user, false);

            return true;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task LogOut(Guid userId)
        {
            await _signInManager.SignOutAsync();
        }
    }
}