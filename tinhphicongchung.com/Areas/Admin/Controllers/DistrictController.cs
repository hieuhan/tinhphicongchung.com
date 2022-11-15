using System;
using System.Collections.Generic;
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
    public class DistrictController : Controller
    {
        // GET: Admin/District

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            byte joinProvinces = 1;
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Districts districts = new Districts
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId,
                ProvinceId = ConstantHelper.DefaultProvinceId
            };

            var statusTask = Status.Static_GetList();
            var districtsGetPageTask = districts.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, joinProvinces, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, districtsGetPageTask);

            DistrictVM model = new DistrictVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                DistrictsList = districtsGetPageTask.Result != null && districtsGetPageTask.Result.Item1.IsAny() ? districtsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, districtsGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            byte joinProvinces = 1;
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Districts districts = new Districts
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId,
                ProvinceId = ConstantHelper.DefaultProvinceId
            };

            var statusTask = Status.Static_GetList();
            var districtsGetPageTask = districts.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, joinProvinces, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, districtsGetPageTask);

            DistrictVM model = new DistrictVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                DistrictsList = districtsGetPageTask.Result != null && districtsGetPageTask.Result.Item1.IsAny() ? districtsGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, districtsGetPageTask.Result.Item2)
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
    
            DistrictEditVM model = new DistrictEditVM
            {
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(DistrictEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Districts district = new Districts
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    district.Description = model.Description;
                district.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    district.DisplayOrder = model.DisplayOrder;
                district.ProvinceId = ConstantHelper.DefaultProvinceId;

                Tuple<string, string> tuple = await district.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-district-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "District", new { Area = "Admin" })))),
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "District", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int districtId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (districtId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Districts districts = await Districts.Static_GetById(myPrincipal.UserName, districtId);

            if (districts == null || districts.DistrictId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var model = new DistrictEditVM
            {
                DistrictId = districts.DistrictId,
                Name = districts.Name,
                Description = districts.Description.DefaultIfEmpty(),
                StatusId = districts.StatusId,
                DisplayOrder = districts.DisplayOrder
            };

            List<Status> statusList = await Status.Static_GetList();

            model.StatusList = statusList.IsAny() ? statusList : new List<Status>();

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(DistrictEditVM model)
        {
            if (model.DistrictId <= 0)
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

                var districts = await Districts.Static_GetById(myPrincipal.UserName, model.DistrictId);

                if (districts == null || districts.DistrictId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                districts.ActBy = myPrincipal.UserName;
                districts.Name = model.Name;
                districts.Description = model.Description.DefaultIfEmpty();
                districts.StatusId = model.StatusId;
                districts.DisplayOrder = model.DisplayOrder;

                Tuple<string, string> tuple = await districts.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "District", new { Area = "Admin" }))))
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
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
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
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "District", new { Area = "Admin" }))))
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