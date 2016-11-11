using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb
{
    /// <summary>
    /// Summary description for Upload
    /// </summary>
    public class Upload : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpPostedFile uploads = context.Request.Files["upload"];
            string ckEditorFuncNum = context.Request["CKEditorFuncNum"];
            string date = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() +
                          DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            string fileExtension = System.IO.Path.GetExtension(uploads.FileName).ToLower();
            string filename = date + fileExtension;
            uploads.SaveAs(context.Server.MapPath(".") + "\\Uploads\\" + filename);
            //provide direct URL here
            string url = "http://" + HttpContext.Current.Request.Url.Authority + "/Uploads/" + filename;
            context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + ckEditorFuncNum + ", \"" + url +
                                   "\");</script>");
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}