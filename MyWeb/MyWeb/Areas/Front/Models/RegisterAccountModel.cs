using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Front.Models
{
    public class RegisterAccountModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên hiển thị")]
        [Display(Name = "Tên")]
        public string DisplayName { get; set; }

        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email sai định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập UserName")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Passwork")]
        public string Password { get; set; }
    }
}