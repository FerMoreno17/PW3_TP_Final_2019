using System.Web;
using System.Web.Optimization;

namespace TP_Final_2019_v._0
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.js",
                        "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/dcjqaccordion").Include(
                        "~/Scripts/jquery.dcjqaccordion.2.7.js"));

            bundles.Add(new ScriptBundle("~/bundles/scrollTo").Include(
                        "~/Scripts/jquery.scrollTo.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/common-scripts").Include(
                        "~/Scripts/common-scripts.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/countdown").Include(
                        "~/Scripts/jquery.countdown.js",
                        "~/Scripts/jquery.countdown.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/barfiller").Include(
                        "~/Scripts/jquery.barfiller.js"));

            bundles.Add(new ScriptBundle("~/bundles/countTo").Include(
                        "~/Scripts/jquery.countTo.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/swiper").Include(
                        "~/Scripts/swiper.js",
                        "~/Scripts/swiper.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/circle-progress").Include(
                        "~/Scripts/circle-progress.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/collapsible").Include(
                        "~/Scripts/jquery.collapsible.min.js",
                        "~/Scripts/jquery.collapsible.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/Scripts/custom.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/elegant-fonts.css",
                      "~/Content/font-awesome.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/swiper.css",
                      "~/Content/swiper.min.css",
                      "~/Content/themify-icons.css",
                      "~/Content/style.css"));   
            
             bundles.Add(new StyleBundle("~/Content/admin_css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/style_admin.css",
                      "~/Content/style_admin-responsive.css"));     
        }
    }
}
