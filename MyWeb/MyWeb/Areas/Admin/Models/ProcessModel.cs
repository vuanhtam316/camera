using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Admin.Models
{
    public class ProcessModel
    {
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public int CameraId { get; set; }
        public string CustomerNumber { get; set; }
        public string CameraName { get; set; }
        public string FunctionName { get; set; }
        public int FunctionId { get; set; }
        public int ?Status { get; set; }
    }
}