using MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Front.Models
{
    public class ViewModel
    {
        //public IEnumerable<CameraModel> Camera { get; set; }
        //public CameraModel Cameramodel { get; set; }

        //public IEnumerable<FunctionsModel> Function { get; set; }
        //public FunctionsModel function { get; set; }

        //public CameraStatus Camerastatus { get; set; }
        public string CAMERA_NAME { get; set; }
        public int CAMERA_ID { get; set; }
        public int CameraStatus_Id { get; set; }
        //public long Camera_Id { get; set; }
        public short Function_Id { get; set; }
        public Nullable<short> Camera_Status { get; set; }
        public Nullable<short> Function_regis { get; set; }
        public string text { get; set; }

        public string month { get; set; }
        public string day { get; set; }
        public int totaFile { get; set; }
        //public short FunctionID { get; set; }
        //public Nullable<short> Status { get; set; }
        //public string text { get; set; }
    }
    public class SelectListCamera
    {
        public int CameraId { get; set; }
        public List<SelectListItem> GetListCameras { get; set; }
    }
}