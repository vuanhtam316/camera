using System.Web.Mvc;
using System.Web.Routing;

namespace MyWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            //        routes.MapRoute(null,
            //           "gioi-thieu",
            //           new
            //           {
            //               controller = "Home",
            //               action = "AboutUs"
            //           }, new[] { "MyWeb.Areas.Front.Controllers" }
            //           ).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(null,
            //           "tai-catalog",
            //           new
            //           {
            //               controller = "DowloadCatalog",
            //               action = "Index"
            //           }, new[] { "MyWeb.Areas.Front.Controllers" }
            //           ).DataTokens.Add("Area", "Front");

            routes.MapRoute(null,
           "Admin",
           new
           {
               controller = "Home",
               action = "Index"
           }, new[] { "MyWeb.Areas.Admin.Controllers" }
           ).DataTokens.Add("Area", "Front");

            routes.MapRoute(null,
           "Enterprise",
           new
           {
               controller = "HomeEnterprise",
               action = "Index"
           }, new[] { "MyWeb.Areas.Enterprise.Controllers" }
           ).DataTokens.Add("Area", "Front");

            routes.MapRoute(null,
           "Agency",
           new
           {
               controller = "HomeAgency",
               action = "Index"
           }, new[] { "MyWeb.Areas.Agency.Controllers" }
           ).DataTokens.Add("Area", "Front");
            //        ////////////

            // routes.MapRoute(null,
            //"dang-nhap",
            //new
            //{
            //    controller = "Home",
            //    action = "Login"
            //}, new[] { "MyWeb.Areas.Admin.Controllers" }
            //).DataTokens.Add("Area", "Admin");

            // routes.MapRoute(null,
            // "dang-xuat",
            // new
            // {
            //     controller = "Home",
            //     action = "Logout"
            // }, new[] { "MyWeb.Areas.Admin.Controllers" }
            // ).DataTokens.Add("Area", "Admin");

            //        routes.MapRoute(null,
            //           "lien-he",
            //           new
            //           {
            //               controller = "Home",
            //               action = "ContactUs"
            //           }, new[] { "MyWeb.Areas.Front.Controllers" }
            //           ).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(null,
            //            "tin-tuc",
            //            new
            //            {
            //                controller = "News",
            //                action = "Index"
            //            }, new[] { "MyWeb.Areas.Front.Controllers" }
            //            ).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(null,
            //            "dang-ky-lai-thu",
            //            new
            //            {
            //                controller = "Home",
            //                action = "RegisterCar"
            //            }, new[] { "MyWeb.Areas.Front.Controllers" }
            //            ).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(null,
            //        //    "dang-nhap",
            //        //    new
            //        //    {
            //        //        controller = "Users",
            //        //        action = "Login"
            //        //    }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //    ).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(null,
            //        //    "thanh-toan",
            //        //    new
            //        //    {
            //        //        controller = "Cart",
            //        //        action = "CheckOut"
            //        //    }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //    ).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(null,
            //        //    "gio-hang",
            //        //    new
            //        //    {
            //        //        controller = "Cart",
            //        //        action = "MyCart"
            //        //    }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //    ).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(null,
            //        //    "dat-hang-thanh-cong",
            //        //    new
            //        //    {
            //        //        controller = "Cart",
            //        //        action = "CheckOutSuccess"
            //        //    }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //    ).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(null,
            //        //    "cart/addtocart",
            //        //    new
            //        //    {
            //        //        controller = "Cart",
            //        //        action = "AddToCart"
            //        //    }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //    ).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(null,
            //        //    "cart/updatecart",
            //        //    new
            //        //    {
            //        //        controller = "Cart",
            //        //        action = "UpdateInCartShop"
            //        //    }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //    ).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(null,
            //        //    "cart/remove",
            //        //    new
            //        //    {
            //        //        controller = "Cart",
            //        //        action = "RemoveCart"
            //        //    }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //    ).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(null,
            //        //    "cart/cartinfo",
            //        //    new
            //        //    {
            //        //        controller = "Cart",
            //        //        action = "CartInfo"
            //        //    }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //    ).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(null,
            //        //   "san-pham/giam-gia",
            //        //   new
            //        //   {
            //        //       controller = "Product",
            //        //       action = "ProductSale",
            //        //       page = 1
            //        //   }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //   ).DataTokens.Add("Area", "Front");


            //        //routes.MapRoute(null,
            //        //   "san-pham/ban-chay",
            //        //   new
            //        //   {
            //        //       controller = "Product",
            //        //       action = "ProductHot",
            //        //       page = 1
            //        //   }, new[] { "MyWeb.Areas.Front.Controllers" }
            //        //   ).DataTokens.Add("Area", "Front");


            //        routes.MapRoute(null,
            //            "san-pham",
            //            new
            //            {
            //                controller = "Product",
            //                action = "Index",
            //                category = (string)null,
            //                page = 1
            //            }, new[] { "MyWeb.Areas.Front.Controllers" }
            //            ).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(null,
            //"tim-kiem",
            //new
            //{
            //    controller = "Home",
            //    action = "SearchProduct",
            //    category = (string)null,
            //    page = 1
            //}, new[] { "MyWeb.Areas.Front.Controllers" }
            //).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(null,
            //            "san-pham/trang-{page}",
            //            new
            //            {
            //                controller = "Product",
            //                action = "Index",
            //                category = (string)null,
            //                page = 1
            //            }, new[] { "MyWeb.Areas.Front.Controllers" }
            //            ).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(
            //            null,
            //           "trang-chu",
            //            new
            //            {
            //                controller = "Home",
            //                action = "Index",
            //                category = UrlParameter.Optional
            //            }, new[] { "MyWeb.Areas.Front.Controllers" }).DataTokens.Add("Area", "Front");

            //        //routes.MapRoute(
            //        //    null,
            //        //    "danh-muc/{menu}/{category}",
            //        //    new
            //        //    {
            //        //        controller = "Home",
            //        //        action = "GetListItemByCategory",
            //        //        category = UrlParameter.Optional
            //        //    }, new[] { "MyWeb.Areas.Front.Controllers" }).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(
            //            null,
            //            "danh-muc/{category}",
            //            new
            //            {
            //                controller = "Product",
            //                action = "ProductByCaregory",
            //                category = UrlParameter.Optional,
            //                page = 1
            //            }, new[] { "MyWeb.Areas.Front.Controllers" }).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(
            //            null,
            //            "danh-muc/{category}/trang-{page}",
            //            new
            //            {
            //                controller = "Product",
            //                action = "ProductByCaregory",
            //                category = (string)null,
            //                page = UrlParameter.Optional
            //            }, new[] { "MyWeb.Areas.Front.Controllers" }).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(
            //            null,
            //            "san-pham/{category}/{name}",
            //            new
            //            {
            //                controller = "Product",
            //                action = "ProductDetails",
            //                name = UrlParameter.Optional
            //            }, new[] { "MyWeb.Areas.Front.Controllers" }).DataTokens.Add("Area", "Front");

            //        routes.MapRoute(
            //           null,
            //           "tin-tuc/{category}/{name}",
            //           new
            //           {
            //               controller = "News",
            //               action = "NewDetail",
            //               name = UrlParameter.Optional
            //           }, new[] { "MyWeb.Areas.Front.Controllers" }).DataTokens.Add("Area", "Front");

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "MyWeb.Areas.Front.Controllers" }
                ).DataTokens.Add("Area", "Front");
        }
    }
}