using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class NewAgencyModel : TBL_USER_ROLE
    {
        //public long Agency_Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên đại lý")]
        [Display(Name = "Tên đại lý")]
        public string Name_Agency { get; set; }

        //[Required(ErrorMessage = "Vui lòng nhập tên hiển thị")]
        //[Display(Name = "Tên")]
        //public string Display { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã số thuế")]
        [Display(Name = "Mã số thuế")]
        //[StringLength(100, ErrorMessage = "Độ dài ít nhất 6 ký tự.", MinimumLength = 6)]
        public string Tax_Code { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        //[StringLength(100, ErrorMessage = "Độ dài ít nhất 6 ký tự.", MinimumLength = 6)]
        public int Phone { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên người đại diện")]
        [Display(Name = "Người đại diện")]
        public string Representation { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        //[Required(ErrorMessage = "Nhập tên người đại diện")]
        [Display(Name = "Ảnh đại diện")]
        public string Image_Url { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email sai định dạng: abc@abc")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [StringLength(100, ErrorMessage = "Độ dài ít nhất 6 ký tự.", MinimumLength = 6)]
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Độ dài ít nhất 6 ký tự.", MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập lại Password")]
        [Compare("Password", ErrorMessage = "Mật khẩu không đúng")]
        public string ConfigPassword { get; set; }
        public short Active { get; set; }
    }
}