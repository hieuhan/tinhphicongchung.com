using System.Web;
using System.Web.Optimization;

namespace tinhphicongchung.com
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/admin/css").Include(
                      "~/areas/admin/assets/css/tabler.min.css",
                      "~/areas/admin/assets/css/demo.css"));

            bundles.Add(new StyleBundle("~/admin/js").Include(
                      "~/areas/admin/assets/js/tabler.min.js"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
