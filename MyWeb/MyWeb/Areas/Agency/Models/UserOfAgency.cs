using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Agency.Models
{
    public class AgencyModel
    {
        public string avatar { get; set; }
        public string Agency_Name { get; set; }

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

        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [Display(Name = "Email")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Email sai định dạng: abc@abc")]
        public string Email { get; set; }
    }



    public class UserOfAgency
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
        [Display(Name = "Tên khách hàng")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression("([0-9][0-9]*)", ErrorMessage = "Nhập số điện thoại.")]
        public string Phone { get; set; }
        public string Username { get; set; }
        public int TotalCamera { get; set; }
        public string Total { get; set; }
        public string SearchName { get; set; }
        public int Agency_Id { get; set; }
    }
    public class CameraOfUser
    {
        public string User_name { get; set; }
        public int Total_Camera { get; set; }


        public int CameraId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên camera.")]
        public string CameraName { get; set; }
        public string FullName { get; set; }
        public int Status_Function { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Username của camera.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Password của camera.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Domain.")]
        public string Domain { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập cổng RTSP.")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Vui lòng nhập cổng RTSP.")]
        public int Port { get; set; }
        public string Camera_Url_Stream { get; set; }
        [Display(Name = "Hãng camera")]
        [Required(ErrorMessage = "Hãy chọn hãng camera.")]
        public string Model_camera { get; set; }

    }
    public class FunctionOfCamera
    {
        public int FunctionID { get; set; }
        public int CameraID { get; set; }
        public string FunctionName { get; set; }
        public int Register { get; set; }
        public int Status { get; set; }

    }
    public class FeedBack
    {
        public int FeedbackID { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
    }
    public class SelectListUserInAgency
    {
        [Required(ErrorMessage = "Vui lòng chọn tên khách hàng")]
        [Display(Name = "Tên khách hàng")]
        public int UserId { get; set; }
        public List<SelectListItem> GetListUser_Agency { get; set; }
    }
}