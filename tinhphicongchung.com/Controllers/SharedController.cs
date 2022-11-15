using System.Web.Mvc;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;
using tinhphicongchung.com.Models;

namespace tinhphicongchung.com.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared

        public ActionResult PartialHeader(BaseViewModel baseViewModel)
        {
            HeaderVM model = new HeaderVM
            {
                Pages = baseViewModel.Pages
            };


            if (Request.IsMobileBrowser())
            {
                if (baseViewModel.MenuItemsList.IsAny())
                {
                    model.HeaderMobileMenusList = baseViewModel.MenuItemsList.FindAll(x => x.MenuId == 1001);
                }

                return PartialView("~/Views/Shared/PartialHeader.Mobile.cshtml", model);
            }

            if (baseViewModel.MenuItemsList.IsAny())
            {
                model.HeaderMenusList = baseViewModel.MenuItemsList.FindAll(x => x.MenuId == 1000);
            }

            return PartialView(model);
        }

        public ActionResult PartialFooter(BaseViewModel baseViewModel)
        {
            int visitorOnline = 0;

            if (HttpContext.Application["VisitorCount"] != null)
            {
                int.TryParse(HttpContext.Application["VisitorCount"].ToString(), out visitorOnline);
            }

            FooterVM model = new FooterVM
            {
                HitCounter = baseViewModel.HitCounter,
                VisitorOnline = visitorOnline,
                Pages = baseViewModel.Pages,
                MenuItemsList = baseViewModel.MenuItemsList
            };

            //HitCounter hitCounter = HitCounter.Static_Get() ?? new HitCounter();

            //BaseViewModel model = new BaseViewModel
            //{
            //    VisitorOnline = visitorOnline,
            //    HitCounter = hitCounter
            //};

            if (Request.IsMobileBrowser())
            {
                return PartialView("~/Views/Shared/PartialFooter.Mobile.cshtml", model);
            }

            return PartialView(model);
        }
    }
}