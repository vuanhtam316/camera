using MyWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Front.Models
{
    public class AddCameraModel
    {
        public IEnumerable<CameraModel> CameraModels { get; set; }
        public CameraModel Cameramodel { get; set; }
        //public int UserId { get; set; }
    }
}