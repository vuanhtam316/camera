using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class FunctionsModel
    {
        public int FunctionId { get; set; }
        public string FunctionName { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public bool Selected { get; set; }
        public string FunctionImage { get; set; }
        //public TBL_USER User { get; set; }
    }
}