using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Agency.Models
{
    public class ListFunctionModel
    {
        public IEnumerable<FunctionOfCamera> ListFunction { get; set; }
        public int CameraId { get; set; }
    }
}