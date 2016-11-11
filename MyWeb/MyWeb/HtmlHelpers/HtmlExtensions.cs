using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using MyWeb.Business;

namespace MyWeb.HtmlHelpers
{
    public static class HtmlExtensions
    {
        public static string HomeMetaTags(string keywords, string description)
        {
            var strMetaTag = new System.Text.StringBuilder();
            strMetaTag.AppendFormat("<meta name=\"keywords\" content=\"{0}\"/>", keywords);
            strMetaTag.AppendFormat("<meta  name=\"description\" content=\"{0}\"/>", description);
            return strMetaTag.ToString();
        }
        //public static string HomeMetaTagsByMenu(string menu)
        //{
        //    string keywords=string.Empty;
        //    string description=string.Empty;
        //    var db = new CameraRemoteEntities();
        //    var menudata = db.Menus.FirstOrDefault(x => x.Tag == menu);
        //    if (menudata != null)
        //    {
        //        keywords = menudata.MetaKeyword;
        //        description = menudata.MetaDescription;
        //    }
        //    else
        //    {
        //        var sys = db.Configs.FirstOrDefault(x => x.Active == true);
        //        if (sys != null)
        //        {
        //            keywords = sys.MetaKeywords;
        //            description = sys.MetaDescriptions;
        //        }
        //    }
        //    var strMetaTag = new System.Text.StringBuilder();
        //    strMetaTag.AppendFormat("<meta name=\"keywords\" content=\"{0}\"/>", keywords);
        //    strMetaTag.AppendFormat("<meta  name=\"descption\" content=\"{0}\"/>", description);
        //    return strMetaTag.ToString();
        //}
        public static MvcHtmlString ValidationHtmlMessageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            return ValidationHtmlMessageFor(helper, expression, string.Empty, (object)null);
        }
        public static MvcHtmlString ValidationHtmlMessageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string validationMessage)
        {
            return ValidationHtmlMessageFor(helper, expression, validationMessage, (object)null);
        }
        public static MvcHtmlString ValidationHtmlMessageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string validationMessage, object htmlAttributes)
        {
            return ValidationHtmlMessageFor(helper, expression, validationMessage, new RouteValueDictionary(htmlAttributes));
        }
        public static MvcHtmlString ValidationHtmlMessageFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string validationMessage, IDictionary<string, object> htmlAttributes)
        {
            return MvcHtmlString.Create(helper.ValidationMessageFor(expression).ToString().Replace("span", "div"));
        }
        public static MvcHtmlString ValidationHtmlMessage(this HtmlHelper htmlHelper, string modelName)
        {
            return MvcHtmlString.Create(htmlHelper.ValidationMessage(modelName).ToString().Replace("span", "div"));
        }
    }
}