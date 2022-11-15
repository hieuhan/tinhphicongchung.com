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
    public class LocationController : Controller
    {
        // GET: Admin/Location

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", int districtId = 0, int wardId = 0, byte landTypeId = 0, int locationTypeId = 0, int streetId = 0, byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Locations location = new Locations
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                LandTypeId = landTypeId,
                LocationTypeId = locationTypeId,
                ProvinceId = ConstantHelper.DefaultProvinceId,
                DistrictId = districtId,
                WardId = wardId,
                StreetId = streetId,
                StatusId = statusId
            };

            byte isJoin = 1;
            var statusTask = Status.Static_GetList();
            var landTypesTask = LandTypes.Static_GetList();
            var locationTypesTask = LocationTypes.Static_GetList(ConstantHelper.StatusIdActivated);
            var locationGetPageTask = location.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, isJoin, page > 0 ? page - 1 : page, pageSize);
            Task<List<Districts>> districtsTask = Districts.Static_GetListByProvince(ConstantHelper.DefaultProvinceId);
            Task<List<Wards>> wardsTask = null;
            Task<List<Streets>> streetsTask = null;

            if (districtId > 0 && wardId > 0)
            {
                wardsTask = Wards.Static_GetListByDistrict(districtId);
                streetsTask = Streets.Static_GetListByWard(wardId);
                
                await Task.WhenAll(districtsTask, wardsTask, streetsTask, statusTask, landTypesTask, locationTypesTask, locationGetPageTask);
            }
            else if (districtId > 0)
            {
                wardsTask = Wards.Static_GetListByDistrict(districtId);
                
                await Task.WhenAll(districtsTask, wardsTask, statusTask, landTypesTask, locationTypesTask, locationGetPageTask);
            }
            else
            {
                await Task.WhenAll(districtsTask, statusTask, landTypesTask, locationTypesTask, locationGetPageTask);
            }

            LocationVM model = new LocationVM
            {
                DistrictsList = districtsTask != null && districtsTask.Result.IsAny() ? districtsTask.Result : new List<Districts>(),
                WardsList = wardsTask != null && wardsTask.Result.IsAny() ? wardsTask.Result : new List<Wards>(),
                StreetsList = streetsTask != null && streetsTask.Result.IsAny() ? streetsTask.Result : new List<Streets>(),
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                LandTypesList = landTypesTask != null && landTypesTask.Result.IsAny() ? landTypesTask.Result : new List<LandTypes>(),
                LocationTypesList = locationTypesTask != null && locationTypesTask.Result.IsAny() ? locationTypesTask.Result : new List<LocationTypes>(),
                LocationsList = locationGetPageTask.Result != null && locationGetPageTask.Result.Item1.IsAny() ? locationGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, locationGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", int districtId = 0, int wardId = 0, byte landTypeId = 0, int locationTypeId = 0, int streetId = 0, byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Locations location = new Locations
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                LandTypeId = landTypeId,
                LocationTypeId = locationTypeId,
                ProvinceId = ConstantHelper.DefaultProvinceId,
                DistrictId = districtId,
                WardId = wardId,
                StreetId = streetId,
                StatusId = statusId
            };

            byte isJoin = 1;
            var statusTask = Status.Static_GetList();
            var landTypesTask = LandTypes.Static_GetList();
            var locationTypesTask = LocationTypes.Static_GetList();
            var locationGetPageTask = location.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, isJoin, page > 0 ? page - 1 : page, pageSize);
       
            await Task.WhenAll(statusTask, landTypesTask, locationTypesTask, locationGetPageTask);
            

            LocationVM model = new LocationVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                LandTypesList = landTypesTask != null && landTypesTask.Result.IsAny() ? landTypesTask.Result : new List<LandTypes>(),
                LocationTypesList = locationTypesTask != null && locationTypesTask.Result.IsAny() ? locationTypesTask.Result : new List<LocationTypes>(),
                LocationsList = locationGetPageTask.Result != null && locationGetPageTask.Result.Item1.IsAny() ? locationGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, locationGetPageTask.Result.Item2)
            };

            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> Add(int districtId = 0, int wardId = 0, int streetId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            var landTypesTask = LandTypes.Static_GetList();
            var locationTypesTask = LocationTypes.Static_GetList(ConstantHelper.StatusIdActivated);
            Task<List<Districts>> districtsTask = Districts.Static_GetListByProvince(ConstantHelper.DefaultProvinceId);
            Task<List<Wards>> wardsTask = null;
            Task<List<Streets>> streetsTask = null;

            if (districtId > 0 && wardId > 0)
            {
                wardsTask = Wards.Static_GetListByDistrict(districtId);
                streetsTask = Streets.Static_GetListByWard(wardId);

                await Task.WhenAll(districtsTask, wardsTask, streetsTask, statusTask, landTypesTask, locationTypesTask);
            }
            else if (districtId > 0)
            {
                wardsTask = Wards.Static_GetListByDistrict(districtId);
                
                await Task.WhenAll(districtsTask, wardsTask, statusTask, landTypesTask, locationTypesTask);
            }
            else
            {
                await Task.WhenAll(districtsTask, statusTask, landTypesTask, locationTypesTask);
            }

            LocationEditVM model = new LocationEditVM
            {
                DistrictsList = districtsTask != null && districtsTask.Result.IsAny() ? districtsTask.Result : new List<Districts>(),
                WardsList = wardsTask != null && wardsTask.Result.IsAny() ? wardsTask.Result : new List<Wards>(),
                StreetsList = streetsTask != null && streetsTask.Result.IsAny() ? streetsTask.Result : new List<Streets>(),
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                LandTypesList = landTypesTask != null && landTypesTask.Result.IsAny() ? landTypesTask.Result : new List<LandTypes>(),
                LocationTypesList = locationTypesTask != null && locationTypesTask.Result.IsAny() ? locationTypesTask.Result : new List<LocationTypes>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(LocationEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Locations location = new Locations
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    location.Description = model.Description;
                location.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    location.DisplayOrder = model.DisplayOrder;
                location.LandTypeId = model.LandTypeId;
                location.LocationTypeId = model.LocationTypeId;
                location.ProvinceId = ConstantHelper.DefaultProvinceId;
                location.DistrictId = model.DistrictId;
                location.WardId = model.WardId;
                location.StreetId = model.StreetId;
                location.Price = double.Parse(model.Price.Replace(",", "").Replace(".", ""));

                Tuple<string, string> tuple = await location.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-location-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "Location", new { Area = "Admin" })))),
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Location", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int locationId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (locationId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            byte isJoin = 1;

            Locations location = await Locations.Static_GetById(myPrincipal.UserName, locationId, isJoin);

            if (location == null || location.LocationId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            var landTypesTask = LandTypes.Static_GetList();
            var locationTypesTask = LocationTypes.Static_GetList(ConstantHelper.StatusIdActivated);
            Task<List<Districts>> districtsTask = Districts.Static_GetListByProvince(ConstantHelper.DefaultProvinceId);
            Task<List<Wards>> wardsTask = null;
            Task<List<Streets>> streetsTask = null;

            if (location.DistrictId > 0 && location.WardId > 0)
            {
                wardsTask = Wards.Static_GetListByDistrict(location.DistrictId);
                streetsTask = Streets.Static_GetListByWard(location.WardId);

                await Task.WhenAll(statusTask, districtsTask, wardsTask, streetsTask, landTypesTask, locationTypesTask);
            }
            else if (location.DistrictId > 0)
            {
                wardsTask = Wards.Static_GetListByDistrict(location.DistrictId);

                await Task.WhenAll(statusTask, districtsTask, wardsTask, landTypesTask, locationTypesTask);
            }
            else
            {
                await Task.WhenAll(statusTask, districtsTask, landTypesTask, locationTypesTask);
            }

            var model = new LocationEditVM
            {
                Name = location.Name,
                Description = location.Description.DefaultIfEmpty(),
                StatusId = location.StatusId,
                DisplayOrder = location.DisplayOrder,
                LandTypeId = location.LandTypeId,
                LocationTypeId = location.LocationTypeId,
                DistrictId = location.DistrictId,
                WardId = location.WardId,
                StreetId = location.StreetId,
                Price = string.Format("{0:0,0}", location.Price),
                DistrictsList = districtsTask != null && districtsTask.Result.IsAny() ? districtsTask.Result : new List<Districts>(),
                WardsList = wardsTask != null && wardsTask.Result.IsAny() ? wardsTask.Result : new List<Wards>(),
                StreetsList = streetsTask != null && streetsTask.Result.IsAny() ? streetsTask.Result : new List<Streets>(),
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                LandTypesList = landTypesTask != null && landTypesTask.Result.IsAny() ? landTypesTask.Result : new List<LandTypes>(),
                LocationTypesList = locationTypesTask != null && locationTypesTask.Result.IsAny() ? locationTypesTask.Result : new List<LocationTypes>(),
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(LocationEditVM model)
        {
            if (model.LocationId <= 0)
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

                byte isJoin = 1;

                Locations location = await Locations.Static_GetById(myPrincipal.UserName, model.LocationId, isJoin);

                if (location == null || location.LocationId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                location.ActBy = myPrincipal.UserName;
                location.Name = model.Name;
                location.Description = model.Description.DefaultIfEmpty();
                location.StatusId = model.StatusId;
                location.DisplayOrder = model.DisplayOrder;
                location.LandTypeId = model.LandTypeId;
                location.LocationTypeId = model.LocationTypeId;
                location.DistrictId = model.DistrictId;
                location.WardId = model.WardId;
                location.StreetId = model.StreetId;
                location.Price = double.Parse(model.Price.Replace(",", "").Replace(".", ""));

                Tuple<string, string> tuple = await location.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Location", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int locationId = 0)
        {
            if (locationId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Locations location = await Locations.Static_GetById(myPrincipal.UserName, locationId);

            if (location == null || location.LocationId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            location.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await location.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "Location", new { Area = "Admin" }))))
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