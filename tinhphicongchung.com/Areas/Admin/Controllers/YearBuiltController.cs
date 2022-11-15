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
    public class YearBuiltController : Controller
    {
        // GET: Admin/YearBuilt

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            YearBuilts yearBuilts = new YearBuilts
            {
                ActBy = myPrincipal.UserName,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var yearBuiltsGetPageTask = yearBuilts.GetPage(keywords, DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, yearBuiltsGetPageTask);

            YearBuiltVM model = new YearBuiltVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                YearBuiltsList = yearBuiltsGetPageTask.Result != null && yearBuiltsGetPageTask.Result.Item1.IsAny() ? yearBuiltsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, yearBuiltsGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            YearBuilts yearBuilts = new YearBuilts
            {
                ActBy = myPrincipal.UserName,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var yearBuiltsGetPageTask = yearBuilts.GetPage(keywords, DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, yearBuiltsGetPageTask);

            YearBuiltVM model = new YearBuiltVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                YearBuiltsList = yearBuiltsGetPageTask.Result != null && yearBuiltsGetPageTask.Result.Item1.IsAny() ? yearBuiltsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, yearBuiltsGetPageTask.Result.Item2)
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

            var statusList = await Status.Static_GetList();

            YearBuiltEditVM model = new YearBuiltEditVM
            {
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(YearBuiltEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                YearBuilts yearBuilts = new YearBuilts
                {
                    ActBy = myPrincipal.UserName
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    yearBuilts.Description = model.Description;
                yearBuilts.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    yearBuilts.DisplayOrder = model.DisplayOrder;
                yearBuilts.Value = model.Value.Value;

                Tuple<string, string> tuple = await yearBuilts.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-yearbuilt-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "YearBuilt", new { Area = "Admin" })))),
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "YearBuilt", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int yearBuiltId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (yearBuiltId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            YearBuilts yearBuilts = await YearBuilts.Static_GetById(myPrincipal.UserName, yearBuiltId);

            if (yearBuilts == null || yearBuilts.YearBuiltId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusList = await Status.Static_GetList();

            YearBuiltEditVM model = new YearBuiltEditVM
            {
                Description = yearBuilts.Description.DefaultIfEmpty(),
                StatusId = yearBuilts.StatusId,
                DisplayOrder = yearBuilts.DisplayOrder,
                Value = yearBuilts.Value,
                StatusList = statusList.IsAny() ? statusList : new List<Status>(),
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(YearBuiltEditVM model)
        {
            if (model.YearBuiltId <= 0)
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

                YearBuilts yearBuilts = await YearBuilts.Static_GetById(myPrincipal.UserName, model.YearBuiltId);

                if (yearBuilts == null || yearBuilts.YearBuiltId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                yearBuilts.ActBy = myPrincipal.UserName;
                yearBuilts.Description = model.Description.DefaultIfEmpty();
                yearBuilts.StatusId = model.StatusId;
                yearBuilts.DisplayOrder = model.DisplayOrder;
                yearBuilts.Value = model.Value.Value;

                Tuple<string, string> tuple = await yearBuilts.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "YearBuilt", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int yearBuiltId = 0)
        {
            if (yearBuiltId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            YearBuilts yearBuilts = await YearBuilts.Static_GetById(myPrincipal.UserName, yearBuiltId);

            if (yearBuilts == null || yearBuilts.YearBuiltId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            yearBuilts.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await yearBuilts.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "YearBuilt", new { Area = "Admin" }))))
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