using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyWeb.Models;

namespace MyWeb.Areas.Admin.Models
{
    public class CustomerAdminModel
    {
        public IEnumerable<CustomerModel> CustomerModel { get; set; }
    }
}