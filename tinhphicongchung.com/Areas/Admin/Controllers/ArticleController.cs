using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using tinhphicongchung.com.Areas.Admin.Models;
using tinhphicongchung.com.Areas.Admin.Services.Extensions;
using tinhphicongchung.com.Areas.Admin.Services.Sercurity;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Controllers
{
    public class ArticleController : Controller
    {
        // GET: Admin/Article
        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte reviewStatusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Articles articles = new Articles
            {
                ActBy = myPrincipal.UserName,
                Title = keywords,
                ReviewStatusId = reviewStatusId
            };

            
            var reviewStatusTask = ReviewStatus.Static_GetList();
            var articlesGetPageTask = articles.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(reviewStatusTask, articlesGetPageTask);

            ArticleVM model = new ArticleVM
            {
                ReviewStatusList = reviewStatusTask != null && reviewStatusTask.Result.IsAny() ? reviewStatusTask.Result : new List<ReviewStatus>(),
                ArticlesList = articlesGetPageTask.Result != null && articlesGetPageTask.Result.Item1.IsAny() ? articlesGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, articlesGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte reviewStatusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Articles articles = new Articles
            {
                ActBy = myPrincipal.UserName,
                Title = keywords,
                ReviewStatusId = reviewStatusId
            };


            var reviewStatusTask = ReviewStatus.Static_GetList();
            var articlesGetPageTask = articles.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(reviewStatusTask, articlesGetPageTask);

            ArticleVM model = new ArticleVM
            {
                ReviewStatusList = reviewStatusTask != null && reviewStatusTask.Result.IsAny() ? reviewStatusTask.Result : new List<ReviewStatus>(),
                ArticlesList = articlesGetPageTask.Result != null && articlesGetPageTask.Result.Item1.IsAny() ? articlesGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, articlesGetPageTask.Result.Item2)
            };

            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            ArticleEditVM model = new ArticleEditVM
            {
                ReviewStatusList = await ReviewStatus.Static_GetList()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(ArticleEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Articles articles = new Articles
                {
                    ActBy = myPrincipal.UserName,
                    Title = model.Title,
                    ArticleUrl = model.ArticleUrl.DefaultIfEmpty(),
                    ArticleContent = model.ArticleContent,
                    MetaTitle = model.MetaTitle.DefaultIfEmpty(),
                    MetaDescription = model.MetaDescription.DefaultIfEmpty(),
                    MetaKeyword = model.MetaKeyword.DefaultIfEmpty(),
                    DisplayOrder = model.DisplayOrder,
                    ImagePath = !model.RemoveImage ? model.ImagePath.DefaultIfEmpty() : string.Empty,
                    ReviewStatusId = ConstantHelper.ReviewStatusIdUnApproved
                };

                Tuple<string, string> tuple = await articles.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-article-form').find('input[type=\"text\"]').val('');app.ckeditorClearValue('Summary');app.ckeditorClearValue('ArticleContent');app.bindTableData('{0}')", Url.Action("BindData", "Article", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Article", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int articleId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (articleId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Articles articles = await Articles.Static_GetById(myPrincipal.UserName, articleId);

            if (articles == null || articles.ArticleId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            ArticleEditVM model = new ArticleEditVM
            {
                ReviewStatusList = await ReviewStatus.Static_GetList()
            };

            model.CopyPropertiesFrom(articles);

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(ArticleEditVM model)
        {
            if (model.ArticleId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Articles articles = await Articles.Static_GetById(myPrincipal.UserName, model.ArticleId);

                if (articles == null || articles.ArticleId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                articles.ActBy = myPrincipal.UserName;
                articles.Title = model.Title;
                articles.ArticleUrl = model.ArticleUrl.DefaultIfEmpty();
                articles.ArticleContent = model.ArticleContent;
                articles.MetaTitle = model.MetaTitle.DefaultIfEmpty();
                articles.MetaDescription = model.MetaDescription.DefaultIfEmpty();
                articles.MetaKeyword = model.MetaKeyword.DefaultIfEmpty();
                articles.DisplayOrder = model.DisplayOrder;
                articles.ImagePath = !model.RemoveImage ? model.ImagePath.DefaultIfEmpty() : string.Empty;

                Tuple<string, string> tuple = await articles.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Article", new { Area = "Admin" }))))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = tuple.Item2
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> Erase(int articleId = 0)
        {
            if (articleId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Articles articles = await Articles.Static_GetById(myPrincipal.UserName, articleId);

            if (articles == null || articles.ArticleId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            articles.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await articles.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "Article", new { Area = "Admin" }))))
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    Completed = false,
                    Message = tuple.Item2
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> MultipleAction(List<int> articleId, byte reviewStatusId = 0)
        {
            if (articleId.IsAny() && reviewStatusId > 0)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Tuple<string, string> tuple = await Articles.Static_UpdateManyReviewStatus(myPrincipal.UserName, string.Join(",", articleId), reviewStatusId);

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Article", new { Area = "Admin" }))))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = tuple.Item2
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }
    }
}