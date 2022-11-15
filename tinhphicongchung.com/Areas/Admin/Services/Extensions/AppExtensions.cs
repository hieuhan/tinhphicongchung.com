using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tinhphicongchung.com.Areas.Admin.Services.Sercurity;

namespace tinhphicongchung.com.Areas.Admin.Services.Extensions
{
    public static class AppExtensions
    {
        public static bool IsAuthenticated
        {
            get
            {
                return HttpContext.Current != null && HttpContext.Current.User != null &&
                       HttpContext.Current.User.Identity != null &&
                       HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public static MyPrincipal GetCurrentUser()
        {
            return IsAuthenticated ? ((MyPrincipal)HttpContext.Current.User) : null;
        }

        //public static string DefaultIfEmpty(this string str, string defaultValue = "", bool considerWhiteSpaceIsEmpty = false)
        //{
        //    return (considerWhiteSpaceIsEmpty ? string.IsNullOrWhiteSpace(str) : string.IsNullOrEmpty(str)) ? defaultValue : str;
        //}
    }
}