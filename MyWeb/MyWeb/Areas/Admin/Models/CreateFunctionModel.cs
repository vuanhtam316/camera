using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Admin.Models
{
    public class CreateFunctionModel
    {
        public int FunctionId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên chức năng")]
        [Display(Name = "Tên chức năng")]
        public string FunctionName { get; set; }
    }
}