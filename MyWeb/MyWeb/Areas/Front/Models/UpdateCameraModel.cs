using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWeb.Models;

namespace MyWeb.Areas.Front.Models
{
    public class UpdateCameraModel
    {
        public CameraModel CameraModel { get; set; }
        public int FunctionId { get; set; }
        public List<FunctionsModel> FunctionsModel { get; set; }
    }
}