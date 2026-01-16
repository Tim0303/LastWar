using System.ComponentModel.DataAnnotations;

namespace NETWebTemp.Service.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class LoginVm
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public LoginVm()
        {
        }

        /// <summary>
        /// 帳號
        /// </summary>
        /// <example>user</example>
        [Required(ErrorMessage = "帳號必填")]
        [StringLength(50, ErrorMessage = "帳號長度不可超過50字")]
        public string Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        /// <example>AAAaaa@@123</example>
        [Required(ErrorMessage = "密碼必填")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "密碼長度需8-100字")]
        [DataType(DataType.Password)]
        public string Secret { get; set; }

        [Required(ErrorMessage = "驗證碼必填")]
        [StringLength(10, ErrorMessage = "驗證碼長度不可超過10字")]
        public string Captcha { get; set; }
    }
}