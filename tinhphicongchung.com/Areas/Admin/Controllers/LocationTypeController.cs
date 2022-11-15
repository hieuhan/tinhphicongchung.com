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
    public class LocationTypeController : Controller
    {
        // GET: Admin/LocationType

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            LocationTypes locationTypes = new LocationTypes
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var LocationTypesGetPageTask = locationTypes.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, LocationTypesGetPageTask);

            LocationTypeVM model = new LocationTypeVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ?  statusTask.Result : new List<Status>(),
                LocationTypesList = LocationTypesGetPageTask.Result != null && LocationTypesGetPageTask.Result.Item1.IsAny() ? LocationTypesGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, LocationTypesGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            LocationTypes locationTypes = new LocationTypes
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var LocationTypesGetPageTask = locationTypes.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, LocationTypesGetPageTask);

            LocationTypeVM model = new LocationTypeVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                LocationTypesList = LocationTypesGetPageTask.Result != null && LocationTypesGetPageTask.Result.Item1.IsAny() ? LocationTypesGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, LocationTypesGetPageTask.Result.Item2)
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

            LocationTypeEditVM model = new LocationTypeEditVM
            {
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(LocationTypeEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                LocationTypes locationTypes = new LocationTypes
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    locationTypes.Description = model.Description;
                locationTypes.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    locationTypes.DisplayOrder = model.DisplayOrder;

                Tuple<string, string> tuple = await locationTypes.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-locationtype-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "LocationType", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "LocationType", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int locationTypeId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (locationTypeId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            LocationTypes locationTypes = await LocationTypes.Static_GetById(myPrincipal.UserName, locationTypeId);

            if (locationTypes == null || locationTypes.LocationTypeId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            List<Status> statusList = await Status.Static_GetList();

            var model = new LocationTypeEditVM
            {
                Name = locationTypes.Name,
                Description = locationTypes.Description.DefaultIfEmpty(),
                StatusId = locationTypes.StatusId,
                DisplayOrder = locationTypes.DisplayOrder,
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(LocationTypeEditVM model)
        {
            if (model.LocationTypeId <= 0)
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

                LocationTypes locationType = await LocationTypes.Static_GetById(myPrincipal.UserName, model.LocationTypeId);

                if (locationType == null || locationType.LocationTypeId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                locationType.ActBy = myPrincipal.UserName;
                locationType.Name = model.Name;
                locationType.Description = model.Description.DefaultIfEmpty();
                locationType.StatusId = model.StatusId;
                locationType.DisplayOrder = model.DisplayOrder;

                Tuple<string, string> tuple = await locationType.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "LocationType", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int locationTypeId = 0)
        {
            if (locationTypeId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            LocationTypes locationTypes = await LocationTypes.Static_GetById(myPrincipal.UserName, locationTypeId);

            if (locationTypes == null || locationTypes.LocationTypeId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            locationTypes.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await locationTypes.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "LocationType", new { Area = "Admin" }))))
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