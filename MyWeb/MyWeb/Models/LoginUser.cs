using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage = "Nhập username")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [Display(Name = "Nhập mật khẩu")]
        public string Password { get; set; }
    }
}