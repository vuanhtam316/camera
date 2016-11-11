using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class CustomerModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Vui lòng tên")]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng họ tên")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng mã khách hàng")]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "Mã khách hàng không hợp lệ")]
        [Display(Name = "Mã khách hàng")]
        public string CustomerNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập username")]
        [Display(Name = "Tên đăng nhập")]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "User name không hợp lệ")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập password")]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Vui lòng tên hiển thị")]
        [Display(Name = "Tên hiển thị")]
        public string DisplayName { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        [Display(Name = "Điện thoại")]
        [RegularExpression("([0-9]*)", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email sai định dạng")]
        public string Email { get; set; }
        public int? CameraId { get; set; }
        public int? TotalCamera { get; set; }
        public short? Status { get; set; }
        [Display(Name = "Giới tính")]
        public bool Sex { get; set; }
        [Display(Name = "Kích hoạt")]
        public bool Active { get; set; }

    }
}