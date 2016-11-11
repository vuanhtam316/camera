using System.Diagnostics;
using System.Web.Mvc;

namespace MyWeb.Areas.Front
{
    public class FrontAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Front";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            Trace.WriteLine("Front");
            //context.MapRoute(null,
            //    "Front/{controller}/{action}/{id}",
            //   new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
