using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Admin.Models
{
    public class RolesModel
    {
        public int RoleId { get; set; }
        [Display(Name = "Tên quyền")]
        [Required(ErrorMessage = "Vui lòng nhập quyền")]
        public string RoleName { get; set; }
        [Display(Name = "Mô tả")]
        public string Description { get; set; }
    }
}