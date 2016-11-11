using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Admin.Models
{
    public class RoleAddToUserModel
    {
        [Required(ErrorMessage = "Vui lòng chọn Quyền")]
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Vui lòng Chọn User")]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<SelectListItem> ListUsers { get; set; }
        public List<SelectListItem> ListRoles { get; set; }
    }
}