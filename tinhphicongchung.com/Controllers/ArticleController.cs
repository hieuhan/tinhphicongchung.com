using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;
using tinhphicongchung.com.Models;

namespace tinhphicongchung.com.Controllers
{
    public class ArticleController : Controller
    {
        readonly CacheHelper cacheHelper = new CacheHelper();

        // GET: Article
        public async Task<ActionResult> Detail()
        {
            string absolutePath = Request.Url.AbsolutePath.Substring(1);

            ArticleVM model = cacheHelper.Get<ArticleVM>(string.Format("ArticleDetail_{0}", absolutePath));

            if(model != null)
            {
                return View(model);
            }

            Articles articles = await Articles.Static_GetViewByUrl(absolutePath);

            if (articles == null || articles.ArticleId <= 0 || string.IsNullOrWhiteSpace(articles.ArticleUrl))
            {
                return RedirectToAction("Error", "Home");
            }

            if (!Request.RawUrl.Contains(articles.ArticleUrl))
            {
                Response.RedirectPermanent(articles.ArticleUrl.GetUrl());
            }

            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result
                };
            }

            model = new ArticleVM
            {
                Articles = articles,
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                WebsiteTitle = articles.MetaTitle.DefaultIfEmpty(articles.Title),
                MetaTitle = articles.MetaTitle.DefaultIfEmpty(articles.Title),
                MetaDescription = articles.MetaDescription.DefaultIfEmpty(articles.Title),
                MetaKeywords = articles.MetaKeyword.DefaultIfEmpty(articles.Title),
                WebsiteImage = articles.ImagePath.GetImageUrl()
            };

            cacheHelper.Set<ArticleVM>(string.Format("ArticleDetail_{0}", absolutePath), model, ConstantHelper.CacheTimeInSecond);

            return View(model);
        }
    }
}