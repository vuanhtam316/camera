using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class NewCustomer : TBL_USER_ROLE

    {
        //public string ROLE_NAME { get; set; }
        //public string USERROLE_DESCRIPTION { get; set; }
        //public int ROLE_ID { get; set; }
        public long USER_ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        public string NAME_USERS { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên doanh nghiệp")]
        [Display(Name = "Tên doanh nghiệp")]
        public string NAME_CUSTOMER { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập tên hiển thị")]
        //[Display(Name = "Tên")]
        public string DISPLAY_NAME { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản")]
        [Display(Name = "Tài khoản")]
        [StringLength(100, ErrorMessage = "Độ dài ít nhất 6 ký tự.", MinimumLength = 6)]
        public string USER_NAME { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [StringLength(100, ErrorMessage = "Độ dài ít nhất 6 ký tự.", MinimumLength = 6)]

        [Display(Name = "Mật khẩu")]
        public string PASSWORDS { get; set; }
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [Compare("PASSWORDS", ErrorMessage = "Mật khẩu không đúng")]
        public string CONFIGPASSWORDS { get; set; }
         [Required(ErrorMessage = "Vui lòng nhập Email")]
        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email sai định dạng: abc@abc")]
        public string EMAIL { get; set; }
        public short USER_ACTIVE { get; set; }

        //[Required(ErrorMessage = "Vui lòng chọn quyền")]
        //public string NAME_ROLE { get; set; }


    }
}