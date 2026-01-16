using NETWebTemp.Service.ViewModels;

namespace NETWebTemp.Service.Interfaces
{
    public interface IAuthenticateService
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        Task<object> Login(LoginVm userModel);

        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        Task LogOut(Guid userId);

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<object> Register(RegisterVm model);

        /// <summary>
        /// RefreshToken
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<object> RefreshToken(RefreshTokenVm model);
    }
}