using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class CameraModel
    {
        public int CAMERA_ID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên camera")]
        [Display(Name = "Tên camera")]
        public string CAMERA_NAME { get; set; }
        //public string CAMERA_NUMBER { get; set; }

        [Display(Name = "Link hình ảnh")]
        //[Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        public string CAMERA_URL { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập Url_Stream ")]
        [Display(Name = "Link Streaming")]
        public string CAMERA_URL_STREAM { get; set; }
        //[Required(ErrorMessage = "Vui lòng Nhập Camera_Ip")]
        //[Display(Name = "Địa chỉ IP")]
        //[Required(ErrorMessage = "Vui lòng nhập IP camera. Ví dụ: 192.168.1.1")]
        //[RegularExpression(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$", ErrorMessage = "Địa chỉ IP không hợp lệ")]
        //public string CAMERA_IP { get; set; }
        ////[Required(ErrorMessage = "Vui lòng nhập Camera_Number")]
        //[Display(Name = "Số cam")]
        //public string CAMERA_NUMBER { get; set; }
        [Display(Name = "Hãng camera")]
        [Required(ErrorMessage = "Hãy chọn hãng camera.")]
        public string MODEL_CAMERA { get; set; }

        [Display(Name = "Username của camera")]
        [Required(ErrorMessage = "Nhập Username của camera.")]
        public string USERNAME_CAMERA { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password của camera")]
        [Required(ErrorMessage = "Nhập Password của camera.")]
        public string PASSWORD_CAMERA { get; set; }

        [Display(Name = "Cổng RTSP")]
        [Required(ErrorMessage = "Nhập cổng RTSP.")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Chỉ được nhập số.")]
        public string PORT_RTSP { get; set; }

        [Display(Name = "Domain")]
        [Required(ErrorMessage = "Nhập domain.")]
        public string IP_CAMERA { get; set; }

        //[Display(Name = "IP camera")]
        //public string CAMERA_IP { get; set; }
        public int USER_ID { get; set; }
    }
}