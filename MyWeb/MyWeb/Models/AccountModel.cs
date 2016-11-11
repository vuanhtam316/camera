using System;
using System.ComponentModel.DataAnnotations;

namespace MyWeb.Models
{
    public class AccountModel
    {
        [Required(ErrorMessage = "Nhập username")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
        [Display(Name = "Nhập mật khẩu cũ")]
        public string Passwords { get; set; }
        //----------------
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        //[StringLength(Int32.MaxValue, MinimumLength = 8, ErrorMessage = "Mật khẩu tối thiểu là 8 ký tự")]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,100}$", ErrorMessage = "Mật khẩu phải có chữ cái và số")]
        [Display(Name = "Nhập mật khẩu mới")]
        public string NewPasswords { get; set; }
        //-----------
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập lại password")]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("NewPasswords", ErrorMessage = "Mật khẩu không khớp nhau")]
        public string ConfirmPasswords { get; set; }
    }
}