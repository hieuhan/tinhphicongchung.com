using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using tinhphicongchung.com.Areas.Admin.Services.Sercurity;
using tinhphicongchung.com.library;
using tinhphicongchung.com.Services;

namespace tinhphicongchung.com
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Application["VisitorCount"] = 0;
            log4net.Config.XmlConfigurator.Configure();
            SchedulerService.StartAsync().GetAwaiter().GetResult();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            Response.Clear();

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string areaName = string.Empty;
                var routeData = HttpContext.Current.Request.RequestContext.RouteData;
                if (routeData.DataTokens["area"] != null)
                    areaName = routeData.DataTokens["area"].ToString();

                Server.ClearError();

                if (!string.IsNullOrEmpty(areaName))
                    Server.TransferRequest("/admin/404.html");
                else Server.TransferRequest("/404.html");
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            try
            {
                Application.Lock();
                HitCounter.Static_Update();
                Application["VisitorCount"] = Convert.ToInt64(Application["VisitorCount"]) + 1;
                Application.UnLock();
            }
            catch (Exception)
            {

            }
        }

        void Session_End(object sender, EventArgs e)
        {
            try
            {
                Application.Lock();
                Application["VisitorCount"] = Convert.ToInt64(Application["VisitorCount"]) - 1;
                Application.UnLock();
            }
            catch (Exception)
            {

            }
        }

    }
}
