using System.Web;
using System.Web.Optimization;

namespace Epam_FinalProject_FileManager
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Используйте версию Modernizr для разработчиков, чтобы учиться работать. Когда вы будете готовы перейти к работе,
            // используйте средство сборки на сайте http://modernizr.com, чтобы выбрать только нужные тесты.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryfileupload").Include(
                "~/Scripts/jquery-ui-1.11.4.min.js",
                "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js",
                "~/Scripts/bootstrap-select.js",
                "~/Scripts/FileUtility.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryfileupload2").Include(
    "~/Scripts/jquery-ui-1.11.4.min.js",
    "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
    "~/Scripts/jQuery.FileUpload/jquery.fileupload.js",
    "~/Scripts/jquery.unobtrusive-ajax.min.js",
    "~/Scripts/bootstrap-select.js",
    "~/Scripts/FileUtility2.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/PagedList.css",
                      //"~/Content/bootstrap-select.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryfileupload").Include("~/Content/jQuery.FileUpload/css/jquery.fileupload.css"));
        }
    }
}
