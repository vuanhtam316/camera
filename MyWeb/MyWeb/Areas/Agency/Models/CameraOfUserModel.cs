using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Agency.Models
{
    public class CameraOfUserModel
    {
        public IEnumerable<CameraOfUser> CameraInUser { get; set; }
        public string Role { get; set; }
    }
    
}