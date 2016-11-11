using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWeb.Models;

namespace MyWeb.Areas.Admin.Models
{
    public class AddFunctionModel
    {
        //[Required(ErrorMessage = "Vui lòng chọn tên function")]
        public int FunctionId { get; set; }
        public int CameraId { get; set; }
        //public List<SelectListItem> FunctionsModel { get; set; }
        public List<FunctionsModel> Functions { get; set; } 
    }
}