using MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Agency.Models
{
    public class UserAgencyModel
    {
        public IEnumerable<UserOfAgency> UserModel { get; set; }
    }
}