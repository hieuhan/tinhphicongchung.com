using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using tinhphicongchung.com.Areas.Admin.Models;
using tinhphicongchung.com.Areas.Admin.Services.Attributes;
using tinhphicongchung.com.Areas.Admin.Services.Extensions;
using tinhphicongchung.com.Areas.Admin.Services.Sercurity;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Controllers
{
    [AuthorizeRole]
    public class LandController : Controller
    {
        // GET: Admin/HomeType

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int constructionLevelId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Lands lands = new Lands
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                ConstructionLevelId = constructionLevelId,
                StatusId = statusId
            };

            byte joinConstructionLevels = 1;
            var statusTask = Status.Static_GetList();
            var constructionLevelsTask = ConstructionLevels.Static_GetList();
            var landsGetPageTask = lands.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, joinConstructionLevels, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, constructionLevelsTask, landsGetPageTask);

            LandVM model = new LandVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>(),
                LandsList = landsGetPageTask.Result != null && landsGetPageTask.Result.Item1.IsAny() ? landsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, landsGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int constructionLevelId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Lands lands = new Lands
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                ConstructionLevelId = constructionLevelId,
                StatusId = statusId
            };

            byte joinConstructionLevels = 1;
            var statusTask = Status.Static_GetList();
            var constructionLevelsTask = ConstructionLevels.Static_GetList();
            var landsGetPageTask = lands.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, joinConstructionLevels, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, constructionLevelsTask, landsGetPageTask);

            LandVM model = new LandVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>(),
                LandsList = landsGetPageTask.Result != null && landsGetPageTask.Result.Item1.IsAny() ? landsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, landsGetPageTask.Result.Item2)
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

            var statusTask = Status.Static_GetList();
            var constructionLevelsTask = ConstructionLevels.Static_GetList();
            await Task.WhenAll(statusTask, constructionLevelsTask);

            LandEditVM model = new LandEditVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(LandEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Lands lands = new Lands
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    lands.Description = model.Description;
                lands.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    lands.DisplayOrder = model.DisplayOrder;
                lands.ConstructionLevelId = model.ConstructionLevelId;
                lands.Price = double.Parse(model.Price.Replace(",", "").Replace(".", ""));

                Tuple<string, string> tuple = await lands.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-land-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "Land", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Land", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int landId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (landId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Lands lands = await Lands.Static_GetById(myPrincipal.UserName, landId);

            if (lands == null || lands.LandId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            var constructionLevelsTask = ConstructionLevels.Static_GetList();
            await Task.WhenAll(statusTask, constructionLevelsTask);

            var model = new LandEditVM
            {
                Name = lands.Name,
                Description = lands.Description.DefaultIfEmpty(),
                StatusId = lands.StatusId,
                DisplayOrder = lands.DisplayOrder,
                ConstructionLevelId = lands.ConstructionLevelId,
                Price = string.Format("{0:0,0}", lands.Price),
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(LandEditVM model)
        {
            if (model.LandId <= 0)
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

                Lands lands = await Lands.Static_GetById(myPrincipal.UserName, model.LandId);

                if (lands == null || lands.LandId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                lands.ActBy = myPrincipal.UserName;
                lands.Name = model.Name;
                lands.Description = model.Description.DefaultIfEmpty();
                lands.StatusId = model.StatusId;
                lands.DisplayOrder = model.DisplayOrder;
                lands.ConstructionLevelId = model.ConstructionLevelId;
                lands.Price = double.Parse(model.Price.Replace(",", "").Replace(".", ""));

                Tuple<string, string> tuple = await lands.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Land", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int landId = 0)
        {
            if (landId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Lands lands = await Lands.Static_GetById(myPrincipal.UserName, landId);

            if (lands == null || lands.LandId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            lands.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await lands.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "Land", new { Area = "Admin" }))))
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
    }
}