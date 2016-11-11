using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Agency
{
    public class AgencyAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Agency";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(null,
                "Agency/{controller}/{action}/{id}",
               new { controller = "HomeAgency", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}