using System.Web.Mvc;
using tinhphicongchung.com.Areas.Admin.Services.Attributes;

namespace tinhphicongchung.com
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleLogErrorAttribute());
            filters.Add(new MyAuthTicketAttribute());
        }
    }
}
