using System.ComponentModel.DataAnnotations;

namespace NETWebTemp.Service.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class TokenVm
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public TokenVm()
        {
        }

        /// <summary>
        /// 原 Token
        /// </summary>
        [Required]
        public string Token { get; set; }

        /// <summary>
        /// Refresh Token
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }

        public DateTime Expiration { get; set; }
    }
}