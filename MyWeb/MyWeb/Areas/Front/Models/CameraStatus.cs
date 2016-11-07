using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Front.Models
{
    public class CameraStatus
    {
        public int CameraStatus_Id { get; set; }
        public long Camera_Id { get; set; }
        public short Function_Id { get; set; }
        public Nullable<short> Function_Regis { get; set; }
        public Nullable<short> Camera_Status { get; set; }
        public string text { get; set; }
    }
}