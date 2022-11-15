﻿using System;
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
    public class VehicleRegistrationYearController : Controller
    {
        // GET: Admin/VehicleRegistrationYear

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            VehicleRegistrationYears vehicleRegistrationYears = new VehicleRegistrationYears
            {
                ActBy = myPrincipal.UserName,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var VehicleRegistrationYearsGetPageTask = vehicleRegistrationYears.GetPage(keywords, DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, VehicleRegistrationYearsGetPageTask);

            VehicleRegistrationYearVM model = new VehicleRegistrationYearVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                VehicleRegistrationYearsList = VehicleRegistrationYearsGetPageTask.Result != null && VehicleRegistrationYearsGetPageTask.Result.Item1.IsAny() ? VehicleRegistrationYearsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, VehicleRegistrationYearsGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            VehicleRegistrationYears vehicleRegistrationYears = new VehicleRegistrationYears
            {
                ActBy = myPrincipal.UserName,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var VehicleRegistrationYearsGetPageTask = vehicleRegistrationYears.GetPage(keywords, DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, VehicleRegistrationYearsGetPageTask);

            VehicleRegistrationYearVM model = new VehicleRegistrationYearVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                VehicleRegistrationYearsList = VehicleRegistrationYearsGetPageTask.Result != null && VehicleRegistrationYearsGetPageTask.Result.Item1.IsAny() ? VehicleRegistrationYearsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, VehicleRegistrationYearsGetPageTask.Result.Item2)
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

            VehicleRegistrationYearEditVM model = new VehicleRegistrationYearEditVM
            {
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(VehicleRegistrationYearEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                VehicleRegistrationYears vehicleRegistrationYears = new VehicleRegistrationYears
                {
                    ActBy = myPrincipal.UserName
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    vehicleRegistrationYears.Description = model.Description;
                vehicleRegistrationYears.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    vehicleRegistrationYears.DisplayOrder = model.DisplayOrder;
                vehicleRegistrationYears.Value = model.Value.Value;

                Tuple<string, string> tuple = await vehicleRegistrationYears.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-vehicle-registration-year-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "VehicleRegistrationYear", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "VehicleRegistrationYear", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int vehicleRegistrationYearId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (vehicleRegistrationYearId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            VehicleRegistrationYears vehicleRegistrationYears = await VehicleRegistrationYears.Static_GetById(myPrincipal.UserName, vehicleRegistrationYearId);

            if (vehicleRegistrationYears == null || vehicleRegistrationYears.VehicleRegistrationYearId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusList = await Status.Static_GetList();

            VehicleRegistrationYearEditVM model = new VehicleRegistrationYearEditVM
            {
                Description = vehicleRegistrationYears.Description.DefaultIfEmpty(),
                StatusId = vehicleRegistrationYears.StatusId,
                DisplayOrder = vehicleRegistrationYears.DisplayOrder,
                Value = vehicleRegistrationYears.Value,
                StatusList = statusList.IsAny() ? statusList : new List<Status>(),
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(VehicleRegistrationYearEditVM model)
        {
            if (model.VehicleRegistrationYearId <= 0)
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

                VehicleRegistrationYears vehicleRegistrationYears = await VehicleRegistrationYears.Static_GetById(myPrincipal.UserName, model.VehicleRegistrationYearId);

                if (vehicleRegistrationYears == null || vehicleRegistrationYears.VehicleRegistrationYearId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                vehicleRegistrationYears.ActBy = myPrincipal.UserName;
                vehicleRegistrationYears.Description = model.Description.DefaultIfEmpty();
                vehicleRegistrationYears.StatusId = model.StatusId;
                vehicleRegistrationYears.DisplayOrder = model.DisplayOrder;
                vehicleRegistrationYears.Value = model.Value.Value;

                Tuple<string, string> tuple = await vehicleRegistrationYears.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "VehicleRegistrationYear", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int VehicleRegistrationYearId = 0)
        {
            if (VehicleRegistrationYearId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            VehicleRegistrationYears vehicleRegistrationYears = await VehicleRegistrationYears.Static_GetById(myPrincipal.UserName, VehicleRegistrationYearId);

            if (vehicleRegistrationYears == null || vehicleRegistrationYears.VehicleRegistrationYearId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            vehicleRegistrationYears.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await vehicleRegistrationYears.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "VehicleRegistrationYear", new { Area = "Admin" }))))
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