using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Front.Models
{
    public class UpdateCameraStatus
    {

        //public  MyProperty { get; set; }
        //public Nullable<short> Status { get; set; }
        
        public CameraStatus Camerastatus { get; set; }
        public List<CameraStatus> CamStatus { get; set; }
    }
}