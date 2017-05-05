using System.Web;
using System.Web.Optimization;

namespace OpmInspection.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.ResetAll();

            /*========== js ==========*/
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/public/js/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/public/js/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/public/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment")
                .Include("~/public/js/moment-with-locales.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable")
                .Include("~/public/js/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/waitforimages")
                .Include("~/public/js/jquery.waitforimages.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/adminlte")
                .Include("~/public/js/app.min.js", "~/public/js/admin-lte-addon.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/auth")
                .Include("~/public/js/auth.min.js"));

            /*========== css ==========*/
            bundles.Add(new StyleBundle("~/css/bootstrap")
                .Include("~/public/css/bootstrap.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/css/miscellaneous")
                .Include("~/public/css/miscellaneous.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/css/font-awesome")
                .Include("~/public/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/css/datatable")
                .Include("~/public/css/jquery.dataTables.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/css/adminlte")
                .Include("~/public/css/adminlte.min.css"));

            bundles.Add(new StyleBundle("~/css/auth")
                .Include("~/public/css/auth.min.css"));

            // Code removed for clarity.
            BundleTable.EnableOptimizations = true;
        }
    }
}
