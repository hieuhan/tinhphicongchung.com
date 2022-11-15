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
    public class WardsController : Controller
    {
        // GET: Admin/Wards

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", int districtId = 0, byte statusId = 0, int page = 0, int pageSize = 200)
        {
            byte isJoin = 1;
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Wards wards = new Wards
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId,
                ProvinceId = ConstantHelper.DefaultProvinceId,
                DistrictId = districtId
            };

            Task<List<Status>> statusTask = Status.Static_GetList();
            Task<List<Districts>> districtsTask = Districts.Static_GetListByProvince(ConstantHelper.DefaultProvinceId);
            var wardsGetPageTask = wards.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, isJoin, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, districtsTask, wardsGetPageTask);

            WardVM model = new WardVM
            {
                DistrictId = districtId,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                DistrictsList = districtsTask != null && districtsTask.Result.IsAny() ? districtsTask.Result : new List<Districts>(),
                WardsList = wardsGetPageTask.Result != null && wardsGetPageTask.Result.Item1.IsAny() ? wardsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, wardsGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", int districtId = 0, byte statusId = 0, int page = 0, int pageSize = 200)
        {
            byte isJoin = 1;
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Wards wards = new Wards
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId,
                ProvinceId = ConstantHelper.DefaultProvinceId,
                DistrictId = districtId
            };

            Task<List<Status>> statusTask = Status.Static_GetList();
            var wardsGetPageTask = wards.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, isJoin, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, wardsGetPageTask);

            WardVM model = new WardVM
            {
                DistrictId = districtId,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                WardsList = wardsGetPageTask.Result != null && wardsGetPageTask.Result.Item1.IsAny() ? wardsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, wardsGetPageTask.Result.Item2)
            };

            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> Add(int districtId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            Task<List<Districts>> districtsTask = Districts.Static_GetListByProvince(ConstantHelper.DefaultProvinceId);

            await Task.WhenAll(statusTask, districtsTask);

            WardEditVM model = new WardEditVM
            {
                DistrictId = districtId,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                DistrictsList = districtsTask != null && districtsTask.Result.IsAny() ? districtsTask.Result : new List<Districts>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(WardEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Wards ward = new Wards
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                ward.Prefix = model.Prefix.DefaultIfEmpty();
                if (!string.IsNullOrWhiteSpace(model.Description))
                    ward.Description = model.Description;
                ward.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    ward.DisplayOrder = model.DisplayOrder;
                ward.ProvinceId = ConstantHelper.DefaultProvinceId;
                ward.DistrictId = model.DistrictId;

                Tuple<string, string> tuple = await ward.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-wards-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "Wards", new { Area = "Admin" })))),
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Wards", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int wardId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (wardId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Wards wards = await Wards.Static_GetById(myPrincipal.UserName, wardId);

            if (wards == null || wards.WardId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var model = new WardEditVM
            {
                WardId = wardId,
                Prefix = wards.Prefix.DefaultIfEmpty(),
                Name = wards.Name,
                Description = wards.Description.DefaultIfEmpty(),
                StatusId = wards.StatusId,
                DisplayOrder = wards.DisplayOrder,
                DistrictId = wards.DistrictId,
            };

            var statusTask = Status.Static_GetList();
            Task<List<Districts>> districtsTask = Districts.Static_GetListByProvince(ConstantHelper.DefaultProvinceId);

            await Task.WhenAll(statusTask, districtsTask);

            model.StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>();
            model.DistrictsList = districtsTask != null && districtsTask.Result.IsAny() ? districtsTask.Result : new List<Districts>();

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(WardEditVM model)
        {
            if (model.WardId <= 0)
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

                var wards = await Wards.Static_GetById(myPrincipal.UserName, model.WardId);

                if (wards == null || wards.WardId <= 0)
                {
                    return Json(new
                    {
                        Completed = false,
                        ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                wards.ActBy = myPrincipal.UserName;
                wards.Prefix = model.Prefix.DefaultIfEmpty();
                wards.Name = model.Name;
                wards.Description = model.Description.DefaultIfEmpty();
                wards.StatusId = model.StatusId;
                wards.DisplayOrder = model.DisplayOrder;
                wards.DistrictId = model.DistrictId;

                Tuple<string, string> tuple = await wards.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Wards", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int districtId = 0)
        {
            if (districtId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Districts districts = await Districts.Static_GetById(myPrincipal.UserName, districtId);

            if (districts == null || districts.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            districts.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await districts.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "Wards", new { Area = "Admin" }))))
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