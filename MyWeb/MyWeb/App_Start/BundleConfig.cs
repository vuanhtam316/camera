using System.Web;
using System.Web.Optimization;

namespace MyWeb
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery-ui.css",
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new StyleBundle("~/Content/Front/Styles").Include(
            "~/Content/Front/Styles/main.css",
            "~/Content/Front/Styles/style.css",
            "~/Content/Admin/styles/HoldOn.css",
            "~/Content/Admin/styles/jquery-ui-1.10.4.custom.min.css",
            "~/Content/Front/Slider_detail/css/nivo-slider.css",
            "~/Content/Front/Slider_detail/css/orman.css",
            "~/Content/Admin/styles/pagging.css",
            "~/Content/Admin/styles/font-awesome.min.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/jqueryslider").Include(
            "~/Content/Front/Slider_detail/Js/jquery.nivo.slider.pack.js",
            "~/Content/Front/Slider_detail/Js/jssor.core.js",
            "~/Content/Front/Slider_detail/Js/jssor.slider.js",
            "~/Content/Front/Slider_detail/Js/jssor.utils.js"
            ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryAjax").Include(
            "~/Scripts/jquery.unobtrusive-ajax.min.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/HoldOn").Include(
            "~/Content/Admin/script/HoldOn.js"
));
            bundles.Add(new ScriptBundle("~/bundles/floater_xlib").Include(
            "~/Scripts/floater_xlib.js"
            ));
        }
    }
}