using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyWeb.Business;

namespace MyWeb.Controllers
{
    public class ErrorController : ProviderControllerBase
    {
        //
        // GET: /Error/

        public ActionResult Index(int? errorCode = null, string errorMsg = default(string))
        {

            try
            {
                switch (errorCode)
                {
                    case (int)HttpStatusCode.NotFound:
                        ViewBag.ErrorTitle = ErrorControllerErrorTitle;//"Page Not Found!";
                        ViewBag.ErrorMsg = ErrorControllerErrorMsg; // "We're sorry, the page you requested cannot be found.";
                        return View();
                    //case (int)HttpStatusCode.Unauthorized:
                    //    if (HttpContext.Session == null || !Request.IsAuthenticated)
                    //    {
                    //        RedirectToAction("SilentLogOff", "Account");
                    //    }
                    //    ViewBag.ErrorTitle = ErrorControllerAccessDenied; //"Access Denied!";
                    //    ViewBag.ErrorMsg = ErrorControllerAccessDenied;
                    //    return View();
                    default:
                        ViewBag.ErrorTitle = ErrorControllerErrorText; // "Error!";
                        ViewBag.ErrorMsg = string.IsNullOrEmpty(errorMsg) ? SerrverErro : errorMsg;
                        return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorTitle = ErrorControllerErrorText; // "Error!";
                Log.Error(ErrorControllerExpFomat, ex);
                return View();
            }
        }

    }
}
