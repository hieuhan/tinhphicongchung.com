using System.Collections.Generic;
using System.Net.Configuration;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;
using tinhphicongchung.com.Models;

namespace tinhphicongchung.com
{
    public class HomeController : Controller
    {
        CacheHelper cacheHelper = new CacheHelper();

        public async Task<ActionResult> Index()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter>  hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result
                };
            }

            BaseViewModel model = new BaseViewModel
            {
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                Pages = cacheVM.Pages
            };

            if(cacheVM.Pages != null)
            {
                model.WebsiteTitle = cacheVM.Pages.MetaTitle;
                model.MetaTitle = cacheVM.Pages.MetaTitle;
                model.MetaDescription = cacheVM.Pages.MetaDescription;
                model.MetaKeywords = cacheVM.Pages.MetaKeyword;
                model.SeoHeader = cacheVM.Pages.H1Tag;
                model.SeoDescription = cacheVM.Pages.Description;
            }

            return View(Request.IsMobileBrowser() ? "Index.Mobile" : "Index", model);
        }

        public ActionResult NotFound()
        {
            return View();
        }
    }
}