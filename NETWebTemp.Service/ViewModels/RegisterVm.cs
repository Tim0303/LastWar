using System.ComponentModel.DataAnnotations;

namespace NETWebTemp.Service.ViewModels
{
    /// <summary>
    ///
    /// </summary>
    public class RegisterVm
    {
        /// <summary>
        /// 建構式
        /// </summary>
        public RegisterVm()
        {
        }

        [Required(ErrorMessage = "帳號必填")]
        [StringLength(50, ErrorMessage = "帳號長度不可超過50字")]
        public string Account { get; set; }

        [Required(ErrorMessage = "信箱必填")]
        [EmailAddress(ErrorMessage = "信箱格式錯誤")]
        public string Email { get; set; }

        [Required(ErrorMessage = "密碼必填")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "密碼長度需8-100字")]
        [DataType(DataType.Password)]
        public string Secret { get; set; }
    }
}