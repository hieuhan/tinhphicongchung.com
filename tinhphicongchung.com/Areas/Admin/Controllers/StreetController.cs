using System;
using System.Collections.Generic;
using System.Linq;
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
    public class StreetController : Controller
    {
        // GET: Admin/Street

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", int districtId = 0, int wardId = 0, byte statusId = 0, int page = 0, int pageSize = 200)
        {
            byte isJoin = 1;
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Streets streets = new Streets
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId,
                ProvinceId = ConstantHelper.DefaultProvinceId,
                DistrictId = districtId,
                WardId = wardId
            };

            var statusTask = Status.Static_GetList();
            var streetsGetPageTask = streets.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, isJoin, page > 0 ? page - 1 : page, pageSize);
            Task<List<Districts>> districtsTask = Districts.Static_GetListByProvince(ConstantHelper.DefaultProvinceId);
            Task<List<Wards>> wardsTask = null;

            if (districtId > 0)
            {
                wardsTask = Wards.Static_GetListByDistrict(districtId);

                await Task.WhenAll(statusTask, districtsTask, wardsTask, streetsGetPageTask);
            }
            else
            {
                await Task.WhenAll(statusTask, districtsTask, streetsGetPageTask);
            }

            StreetVM model = new StreetVM
            {
                DistrictId = districtId,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                DistrictsList = districtsTask != null && districtsTask.Result.IsAny() ? districtsTask.Result : new List<Districts>(),
                WardsList = wardsTask != null && wardsTask.Result.IsAny() ? wardsTask.Result : new List<Wards>(),
                StreetsList = streetsGetPageTask.Result != null && streetsGetPageTask.Result.Item1.IsAny() ? streetsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, streetsGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", int districtId = 0, int wardId = 0, byte statusId = 0, int page = 0, int pageSize = 200)
        {
            byte isJoin = 1;
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Streets streets = new Streets
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId,
                ProvinceId = ConstantHelper.DefaultProvinceId,
                DistrictId = districtId,
                WardId = wardId
            };

            var statusTask = Status.Static_GetList();
            var streetsGetPageTask = streets.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, isJoin, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, streetsGetPageTask);

            StreetVM model = new StreetVM
            {
                DistrictId = districtId,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                StreetsList = streetsGetPageTask != null && streetsGetPageTask.Result.Item1.IsAny() ? streetsGetPageTask.Result.Item1 : new List<Streets>(),
                Pagination = new PaginationVM(page, pageSize, streetsGetPageTask.Result.Item2)
            };

            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> Add(int districtId = 0, int wardId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            Task<List<Districts>> districtsTask = Districts.Static_GetListByProvince(ConstantHelper.DefaultProvinceId);
            Task<List<Wards>> wardsTask = null;

            if (districtId > 0)
            {
                wardsTask = Wards.Static_GetListByDistrict(districtId);

                await Task.WhenAll(statusTask, districtsTask, wardsTask);
            }
            else
            {
                await Task.WhenAll(statusTask, districtsTask);
            }

            StreetEditVM model = new StreetEditVM
            {
                DistrictId = districtId,
                WardId = wardId,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                DistrictsList = districtsTask != null && districtsTask.Result.IsAny() ? districtsTask.Result : new List<Districts>(),
                WardsList = wardsTask != null && wardsTask.Result.IsAny() ? wardsTask.Result : new List<Wards>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(StreetEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Streets streets = new Streets
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name,
                    Prefix = model.Prefix.DefaultIfEmpty(),
                    Description = model.Description.DefaultIfEmpty(),
                    StatusId = model.StatusId,
                    ProvinceId = ConstantHelper.DefaultProvinceId,
                    DistrictId = model.DistrictId,
                    WardId = model.WardId
                };

                if (model.DisplayOrder.HasValue)
                    streets.DisplayOrder = model.DisplayOrder;

                Tuple<string, string> tuple = await streets.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-street-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "Street", new { Area = "Admin" })))),
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Street", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
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
        public async Task<ActionResult> Edit(int streetId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (streetId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Streets streets = await Streets.Static_GetById(myPrincipal.UserName, streetId);

            if (streets == null || streets.StreetId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            Task<List<Districts>> districtsTask = Districts.Static_GetListByProvince(ConstantHelper.DefaultProvinceId);
            Task<List<Wards>> wardsTask = null;

            if (streets.DistrictId > 0)
            {
                wardsTask = Wards.Static_GetListByDistrict(streets.DistrictId);

                await Task.WhenAll(statusTask, districtsTask, wardsTask);
            }
            else
            {
                await Task.WhenAll(statusTask, districtsTask);
            }

            var model = new StreetEditVM
            {
                Name = streets.Name,
                Prefix = streets.Prefix.DefaultIfEmpty(),
                Description = streets.Description.DefaultIfEmpty(),
                DistrictId = streets.DistrictId,
                WardId = streets.WardId,
                StatusId = streets.StatusId,
                DisplayOrder = streets.DisplayOrder,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                DistrictsList = districtsTask != null && districtsTask.Result.IsAny() ? districtsTask.Result : new List<Districts>(),
                WardsList = wardsTask != null && wardsTask.Result.IsAny() ? wardsTask.Result : new List<Wards>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(StreetEditVM model)
        {
            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Streets streets = await Streets.Static_GetById(myPrincipal.UserName, model.StreetId);

                if (streets == null || streets.StreetId <= 0)
                {
                    return Json(new
                    {
                        Completed = false,
                        ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                streets.ActBy = myPrincipal.UserName;
                streets.Name = model.Name;
                streets.Prefix = model.Prefix.DefaultIfEmpty();
                streets.Description = model.Description.DefaultIfEmpty();
                streets.DistrictId = model.DistrictId;
                streets.WardId = model.WardId;
                streets.StatusId = model.StatusId;
                streets.DisplayOrder = model.DisplayOrder;

                Tuple<string, string> tuple = await streets.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Street", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int streetId = 0)
        {
            if (streetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Streets streets = await Streets.Static_GetById(myPrincipal.UserName, streetId);

            if (streets == null || streets.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            streets.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await streets.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "Street", new { Area = "Admin" }))))
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