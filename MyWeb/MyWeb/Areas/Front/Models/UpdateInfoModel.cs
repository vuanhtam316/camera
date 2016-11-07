using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Front.Models
{
    public class UpdateInfoModel
    {
        public string UserName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ID userinfo")]
        public int IdUserInfo { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ID user")]
        public int IdUser { get; set; }

        [Required(ErrorMessage = "Vui lòng họ tên")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ")]
        //[Display(Name = "Nhập mật khẩu cũ")]
        //public string Passwords { get; set; }
        //----------------
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
        //[StringLength(Int32.MaxValue, MinimumLength = 8, ErrorMessage = "Mật khẩu tối thiểu là 8 ký tự")]
        //[RegularExpression(@"^(?=.*\d)(?=.*[a-zA-Z]).{6,100}$", ErrorMessage = "Mật khẩu phải có chữ cái và số")]
        //[Display(Name = "Nhập mật khẩu mới")]
        //public string NewPasswords { get; set; }
        //-----------
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Vui lòng nhập lại password")]
        //[Display(Name = "Xác nhận mật khẩu")]
        //[Compare("NewPasswords", ErrorMessage = "Mật khẩu không khớp nhau")]
        //public string ConfirmPasswords { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Điện thoại")]
        [RegularExpression("([0-9]*)", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email sai định dạng")]
        public string Email { get; set; }
    }
}