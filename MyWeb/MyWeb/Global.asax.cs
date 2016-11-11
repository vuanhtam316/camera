using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using MyWeb.Controllers;
using MyWeb.Business;
using System.Configuration;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;

namespace MyWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MvcApplication));
        //string conStr = @"User Id=REMOTECAMERA;Password=11012253;Data Source=10.62.1.150:1521/camera";
        //public OracleConnection con = null;

        protected void Application_Start()
        {
            //con = new OracleConnection(conStr);
            //Kiểm tra nếu chưa tồn tại file thì tạo file Count_Visited.txt
            //if (!System.IO.File.Exists(Server.MapPath("~/Count_Visited.txt")))
            //{
            //    System.IO.File.WriteAllText(Server.MapPath("~/Count_Visited.txt"), "0");
            //}
            //Application["SoLuotTruyCap"] = int.Parse(System.IO.File.ReadAllText(Server.MapPath("~/Count_Visited.txt")));
            //--------


            AreaRegistration.RegisterAllAreas();
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();

            //here in Application Start we will start Sql Dependency
            //OracleDependency.Port(1521);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Log.Error("App_Error", ex);
            //-----
            var httpContext = ((MvcApplication)sender).Context;
            var currentController = string.Empty;
            var currentAction = string.Empty;
            var currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    currentAction = currentRouteData.Values["action"].ToString();
                }

                var errorMsg = string.Format("An unhandled exception occurs in the controller:{0}, action:{1}", currentController, currentAction);
                Log.Error(errorMsg, ex);
            }

            var controller = new ErrorController();
            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "Index");

            var httpException = ex as HttpException;
            routeData.Values["errorCode"] = (httpException != null) ? (int?)httpException.GetHttpCode() : null;

            httpContext.ClearError();
            Server.ClearError();
            httpContext.Response.Clear();
            Response.ContentType = "text/html";
            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
        //public int userid = GetUserLogin.CurentCustomerId;
        void Session_Start(object sender, EventArgs e)
        {
            NotificationComponent NC = new NotificationComponent();
            var currentTime = DateTime.Now;
            //var userid = GetUserLogin.CurentCustomerId;
            HttpContext.Current.Session["LastUpdated"] = currentTime;
            //HttpContext.Current.Session["UserLogin"]=userid;
            NC.RegisterNotification(currentTime);

            //Application["SoLuotTruyCap"] = (int)Application["SoLuotTruyCap"] + 1;
            //System.IO.File.WriteAllText(Server.MapPath("~/Count_Visited.txt"),
            //Application["SoLuotTruyCap"].ToString());
            //if (Application["slonline"] == null)
            //{
            //    Application["slonline"] = 1;
            //}
            //else
            //{
            //    Application["slonline"] = (int)Application["slonline"] + 1;
            //}
        }
        void Session_End(object sender, EventArgs e)
        {

            //SqlDependency.Stop(conStr);
            //Application["slonline"] = (int)Application["slonline"] - 1;
        }
    }

}