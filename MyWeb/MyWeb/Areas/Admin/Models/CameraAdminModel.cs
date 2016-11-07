using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWeb.Models;

namespace MyWeb.Areas.Admin.Models
{
    public class CameraAdminModel
    {
        public IEnumerable<CameraModel> CameraModels { get; set; }
        public CustomerModel CustomerModel { get; set; }
        public int UserId { get; set; }
    }
}