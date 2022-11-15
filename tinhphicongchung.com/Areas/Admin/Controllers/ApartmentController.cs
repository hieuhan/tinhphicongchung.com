using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
    public class ApartmentController : Controller
    {
        // GET: Admin/Apartment

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int constructionLevelId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Apartments apartments = new Apartments
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                ConstructionLevelId = constructionLevelId,
                StatusId = statusId
            };

            byte joinConstructionLevels = 1;
            var statusTask = Status.Static_GetList();
            var constructionLevelsTask = ConstructionLevels.Static_GetList();
            var apartmentsGetPageTask = apartments.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, joinConstructionLevels, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, constructionLevelsTask, apartmentsGetPageTask);

            ApartmentVM model = new ApartmentVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>(),
                ApartmentsList = apartmentsGetPageTask.Result != null && apartmentsGetPageTask.Result.Item1.IsAny() ? apartmentsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, apartmentsGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int constructionLevelId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Apartments apartments = new Apartments
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                ConstructionLevelId = constructionLevelId,
                StatusId = statusId
            };

            byte joinConstructionLevels = 1;
            var statusTask = Status.Static_GetList();
            var constructionLevelsTask = ConstructionLevels.Static_GetList();
            var apartmentsGetPageTask = apartments.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, joinConstructionLevels, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, constructionLevelsTask, apartmentsGetPageTask);

            ApartmentVM model = new ApartmentVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>(),
                ApartmentsList = apartmentsGetPageTask.Result != null && apartmentsGetPageTask.Result.Item1.IsAny() ? apartmentsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, apartmentsGetPageTask.Result.Item2)
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

            ApartmentEditVM model = new ApartmentEditVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(ApartmentEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Apartments apartments = new Apartments
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    apartments.Description = model.Description;
                apartments.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    apartments.DisplayOrder = model.DisplayOrder;
                apartments.ConstructionLevelId = model.ConstructionLevelId;
                apartments.ApartmentUnitPrice = double.Parse(model.ApartmentUnitPrice.Replace(",", "").Replace(".", ""));
                apartments.MultiPurposeHouseUnitPrice = double.Parse(model.MultiPurposeHouseUnitPrice.Replace(",", "").Replace(".", ""));

                Tuple<string, string> tuple = await apartments.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-apartment-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "Apartment", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Apartment", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int apartmentId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (apartmentId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Apartments apartments = await Apartments.Static_GetById(myPrincipal.UserName, apartmentId);

            if (apartments == null || apartments.ApartmentId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            var constructionLevelsTask = ConstructionLevels.Static_GetList();
            await Task.WhenAll(statusTask, constructionLevelsTask);

            ApartmentEditVM model = new ApartmentEditVM
            {
                Name = apartments.Name,
                Description = apartments.Description.DefaultIfEmpty(),
                StatusId = apartments.StatusId,
                DisplayOrder = apartments.DisplayOrder,
                ConstructionLevelId = apartments.ConstructionLevelId,
                ApartmentUnitPrice = string.Format("{0:0,0}", apartments.ApartmentUnitPrice),
                MultiPurposeHouseUnitPrice = string.Format("{0:0,0}", apartments.MultiPurposeHouseUnitPrice),
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(ApartmentEditVM model)
        {
            if (model.ApartmentId <= 0)
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

                Apartments apartments = await Apartments.Static_GetById(myPrincipal.UserName, model.ApartmentId);

                if (apartments == null || apartments.ApartmentId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                apartments.ActBy = myPrincipal.UserName;
                apartments.Name = model.Name;
                apartments.Description = model.Description.DefaultIfEmpty();
                apartments.StatusId = model.StatusId;
                apartments.DisplayOrder = model.DisplayOrder;
                apartments.ConstructionLevelId = model.ConstructionLevelId;
                apartments.ApartmentUnitPrice = double.Parse(model.ApartmentUnitPrice.Replace(",", "").Replace(".", ""));
                apartments.MultiPurposeHouseUnitPrice = double.Parse(model.MultiPurposeHouseUnitPrice.Replace(",", "").Replace(".", ""));

                Tuple<string, string> tuple = await apartments.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Apartment", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int apartmentId = 0)
        {
            if (apartmentId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Apartments apartments = await Apartments.Static_GetById(myPrincipal.UserName, apartmentId);

            if (apartments == null || apartments.ApartmentId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            apartments.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await apartments.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "Apartment", new { Area = "Admin" }))))
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