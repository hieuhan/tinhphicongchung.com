using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace tinhphicongchung.com.Areas.Admin.Services.Attributes
{
    public class HandleLogErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

                filterContext.Result = new JsonResult
                {
                    Data = new { 
                        Completed = false, 
                        Message = "Quý khách vui lòng thử lại sau.",
                        ReturnUrl = filterContext.HttpContext.Request.UrlReferrer != null && filterContext.HttpContext.Request.UrlReferrer.PathAndQuery.IndexOf("login.html", StringComparison.Ordinal) < 0 ? string.Format("{0}?ReturnUrl={1}", FormsAuthentication.LoginUrl, filterContext.HttpContext.Request.UrlReferrer.PathAndQuery) : string.Concat(FormsAuthentication.LoginUrl, "?ReturnUrl=/")
                    },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    ContentType = "application/json",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(
                    new
                    {
                        controller = "Home",
                        action = "NotFound",
                        area = ""
                    }));

                string areaName = string.Empty;

                if (filterContext.RouteData.DataTokens["area"] != null)
                {
                    areaName = filterContext.RouteData.DataTokens["area"].ToString();
                }

                if (!string.IsNullOrWhiteSpace(areaName))
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Error",
                        action = "NotFound",
                        area = areaName
                    }));
                }
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}