using System.Web.Optimization;

namespace RadCms.Core
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/common/js").Include(
                        "~/Core/Scripts/jquery-{version}.js",
                        "~/Core/Scripts/placeholders.min.js"));
            bundles.Add(new ScriptBundle("~/plugins/js").Include(
                        "~/Core/Scripts/common.js",
                        "~/Core/Scripts/respond.js",
                        "~/Core/Scripts/browser.js"));

            bundles.Add(new ScriptBundle("~/cms/js").Include(
                "~/Core/Content/k/js/kendo.ui.core.min.js"));

            bundles.Add(new ScriptBundle("~/kendo/web/js").Include(
                "~/Core/Content/k/js/kendo.ui.core.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Core/Scripts/jquery.unobtrusive*",
                        "~/Core/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Core/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/common/normalize").Include("~/Core/assets/css/normalize.min.css"));
            bundles.Add(new StyleBundle("~/common/main").Include("~/Core/assets/css/main.css", new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/common/css").Include(
                "~/Core/assets/css/layout.css",
                "~/Core/assets/css/content.css"));

            var theme = Helpers.CmsHelper.Site;
            bundles.Add(new StyleBundle("~/theme/css").Include(
                "~/Core/themes/" + theme + "/css/*.css", new CssRewriteUrlTransform()));
            
#if CMS
            bundles.Add(new StyleBundle("~/cms/css").Include(
                "~/Core/assetsCms/css/cmsBar.css", new CssRewriteUrlTransform()));
            bundles.Add(new StyleBundle("~/cms/sprites").Include("~/Core/assetsCms/css/themes/base/sprites.css", new CssRewriteUrlTransform()));
#endif
            bundles.Add(new StyleBundle("~/kendo/css")
                .Include("~/Core/Content/k/styles/kendo.common.min.css")
                .Include("~/Core/Content/k/styles/kendo.silver.min.css", new CssRewriteUrlTransform()));

            bundles.Add(new StyleBundle("~/cms/kendo").Include("~/Core/Content/k/styles/kendo.common.min.css"));
            bundles.Add(new StyleBundle("~/cms/kendoSilver").Include("~/Core/Content/k/styles/kendo.silver.min.css", new CssRewriteUrlTransform()));

            BundleTable.EnableOptimizations = true;
#if DEBUG
            BundleTable.EnableOptimizations = false;
#endif
        }
    }
}