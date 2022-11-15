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
    public class ConstructionLevelController : Controller
    {
        // GET: Admin/ConstructionLevel

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            ConstructionLevels constructionLevels = new ConstructionLevels
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var constructionLevelsGetPageTask = constructionLevels.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, constructionLevelsGetPageTask);

            ConstructionLevelVM model = new ConstructionLevelVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsGetPageTask.Result != null && constructionLevelsGetPageTask.Result.Item1.IsAny() ? constructionLevelsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, constructionLevelsGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            ConstructionLevels constructionLevels = new ConstructionLevels
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var constructionLevelsGetPageTask = constructionLevels.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, constructionLevelsGetPageTask);

            ConstructionLevelVM model = new ConstructionLevelVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsGetPageTask.Result != null && constructionLevelsGetPageTask.Result.Item1.IsAny() ? constructionLevelsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, constructionLevelsGetPageTask.Result.Item2)
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

            List<Status> statusList = await Status.Static_GetList();

            ConstructionLevelEditVM model = new ConstructionLevelEditVM
            {
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(ConstructionLevelEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                ConstructionLevels constructionLevels = new ConstructionLevels
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    constructionLevels.Description = model.Description;
                constructionLevels.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    constructionLevels.DisplayOrder = model.DisplayOrder;

                Tuple<string, string> tuple = await constructionLevels.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-constructionlevel-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "ConstructionLevel", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "ConstructionLevel", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int constructionLevelId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (constructionLevelId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            ConstructionLevels constructionLevels = await ConstructionLevels.Static_GetById(myPrincipal.UserName, constructionLevelId);

            if (constructionLevels == null || constructionLevels.ConstructionLevelId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            List<Status> statusList = await Status.Static_GetList();

            var model = new ConstructionLevelEditVM
            {
                Name = constructionLevels.Name,
                Description = constructionLevels.Description.DefaultIfEmpty(),
                StatusId = constructionLevels.StatusId,
                DisplayOrder = constructionLevels.DisplayOrder,
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(ConstructionLevelEditVM model)
        {
            if (model.ConstructionLevelId <= 0)
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

                var constructionLevel = await ConstructionLevels.Static_GetById(myPrincipal.UserName, model.ConstructionLevelId);

                if (constructionLevel == null || constructionLevel.ConstructionLevelId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                constructionLevel.ActBy = myPrincipal.UserName;
                constructionLevel.Name = model.Name;
                constructionLevel.Description = model.Description.DefaultIfEmpty();
                constructionLevel.StatusId = model.StatusId;
                constructionLevel.DisplayOrder = model.DisplayOrder;

                Tuple<string, string> tuple = await constructionLevel.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "ConstructionLevel", new { Area = "Admin" }))))
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

        [HttpGet]
        public async Task<JsonResult> Erase(int constructionLevelId = 0)
        {
            if (constructionLevelId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            ConstructionLevels constructionLevels = await ConstructionLevels.Static_GetById(myPrincipal.UserName, constructionLevelId);

            if (constructionLevels == null || constructionLevels.ConstructionLevelId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            constructionLevels.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await constructionLevels.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "ConstructionLevel", new { Area = "Admin" }))))
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