using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class ApplicationUser: UserInfoModel
    {
        public string Email { get; set; }
        public bool ConfirmedEmail { get; set; } 
    }
}