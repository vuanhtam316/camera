using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Front.Models
{
    public class Camera_Function
    {
        public int functionId { get; set; }
        public int userId { get; set; }
        public int cameraId { get; set; }
        public string polygon { get; set; }
        public string urlImage { get; set; }
    }
}