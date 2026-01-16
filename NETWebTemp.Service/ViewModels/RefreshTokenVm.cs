using System.ComponentModel.DataAnnotations;

namespace NETWebTemp.Service.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class RefreshTokenVm
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public RefreshTokenVm()
        {
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }
    }
}