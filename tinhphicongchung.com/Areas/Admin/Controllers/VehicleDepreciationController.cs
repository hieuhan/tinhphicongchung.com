using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
    public class VehicleDepreciationController : Controller
    {
        // GET: Admin/VehicleDepreciation

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            VehicleDepreciation vehicleDepreciation = new VehicleDepreciation
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var depreciationGetPageTask = vehicleDepreciation.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, depreciationGetPageTask);

            VehicleDepreciationVM model = new VehicleDepreciationVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                VehicleDepreciationList = depreciationGetPageTask.Result != null && depreciationGetPageTask.Result.Item1.IsAny() ? depreciationGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, depreciationGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            VehicleDepreciation vehicleDepreciation = new VehicleDepreciation
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var depreciationGetPageTask = vehicleDepreciation.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, depreciationGetPageTask);

            VehicleDepreciationVM model = new VehicleDepreciationVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                VehicleDepreciationList = depreciationGetPageTask.Result != null && depreciationGetPageTask.Result.Item1.IsAny() ? depreciationGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, depreciationGetPageTask.Result.Item2)
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

            VehicleDepreciationEditVM model = new VehicleDepreciationEditVM
            {
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(VehicleDepreciationEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                VehicleDepreciation vehicleDepreciation = new VehicleDepreciation
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    vehicleDepreciation.Description = model.Description;
                vehicleDepreciation.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    vehicleDepreciation.DisplayOrder = model.DisplayOrder;
                vehicleDepreciation.Price = double.Parse(model.Price.Replace(".", "").Replace(".", ""));

                int minValue = 0, maxValue = 0;
                string nameLower = model.Name.Trim().ToLower(), patternBelow = @"dưới\D*([0-9]+)",
                    patternAbove = @"trên\D*([0-9]+)",
                    patternFromTo = @"từ\D*([0-9]+)\D*đến\D*([0-9]+)",
                    patternOverTo = @"trên\D*([0-9]+)\D*đến\D*([0-9]+)";

                Regex regex = new Regex(patternBelow);
                Match match = regex.Match(nameLower);

                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out maxValue);
                    if (maxValue > 0)
                    {
                        maxValue -= 1;
                    }
                }
                else
                {
                    regex = new Regex(patternFromTo);
                    match = regex.Match(nameLower);

                    if (match.Success)
                    {
                        int.TryParse(match.Groups[1].Value, out minValue);
                        int.TryParse(match.Groups[2].Value, out maxValue);
                    }
                    else
                    {
                        regex = new Regex(patternOverTo);
                        match = regex.Match(nameLower);

                        if (match.Success)
                        {
                            int.TryParse(match.Groups[1].Value, out minValue);
                            int.TryParse(match.Groups[2].Value, out maxValue);
                            if (minValue > 0 && maxValue > 0)
                            {
                                minValue += 1;
                            }
                        }
                        else
                        {
                            regex = new Regex(patternAbove);
                            match = regex.Match(nameLower);

                            if (match.Success)
                            {
                                int.TryParse(match.Groups[1].Value, out minValue);
                                if (minValue > 0)
                                {
                                    minValue += 1;
                                }
                                maxValue = 1000;
                            }
                        }
                    }
                }

                if (minValue > 0 || maxValue > 0)
                {
                    if (minValue < maxValue || maxValue == 0)
                    {
                        vehicleDepreciation.MinValue = minValue;
                        vehicleDepreciation.MaxValue = maxValue;

                        Tuple<string, string> tuple = await vehicleDepreciation.Insert();

                        if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                        {
                            if (model.AddAnother)
                            {
                                return Json(new
                                {
                                    Completed = true,
                                    Message = tuple.Item2,
                                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-vehicledepreciation-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "VehicleDepreciation", new { Area = "Admin" }))))
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new
                                {
                                    Completed = true,
                                    Message = tuple.Item2,
                                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "VehicleDepreciation", new { Area = "Admin" }))))
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
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = "Thời gian sử dụng không hợp lệ, thời gian bắt đầu phải nhỏ hơn thời gian kết thúc. </br> Vui lòng nhập dạng: </br> - Dưới x năm </br> - Từ x đến y năm </br> - Trên x năm đến y năm </br> - Trên x năm",
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = "Thời gian sử dụng không hợp lệ. </br> Vui lòng nhập dạng: </br> - Dưới x năm </br> - Từ x đến y năm </br> - Trên x năm đến y năm </br> - Trên x năm",
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
        public async Task<ActionResult> Edit(int vehicleDepreciationId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (vehicleDepreciationId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            VehicleDepreciation vehicleDepreciation = await VehicleDepreciation.Static_GetById(myPrincipal.UserName, vehicleDepreciationId);

            if (vehicleDepreciation == null || vehicleDepreciation.VehicleDepreciationId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusList = await Status.Static_GetList();

            var model = new VehicleDepreciationEditVM
            {
                Name = vehicleDepreciation.Name,
                Description = vehicleDepreciation.Description.DefaultIfEmpty(),
                StatusId = vehicleDepreciation.StatusId,
                DisplayOrder = vehicleDepreciation.DisplayOrder,
                Price = string.Format("{0:0,0}", vehicleDepreciation.Price),
                StatusList = statusList.IsAny() ? statusList : new List<Status>(),
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(VehicleDepreciationEditVM model)
        {
            if (model.VehicleDepreciationId <= 0)
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

                VehicleDepreciation vehicleDepreciation = await VehicleDepreciation.Static_GetById(myPrincipal.UserName, model.VehicleDepreciationId);

                if (vehicleDepreciation == null || vehicleDepreciation.VehicleDepreciationId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                vehicleDepreciation.ActBy = myPrincipal.UserName;
                vehicleDepreciation.Name = model.Name;
                vehicleDepreciation.Description = model.Description.DefaultIfEmpty();
                vehicleDepreciation.StatusId = model.StatusId;
                vehicleDepreciation.DisplayOrder = model.DisplayOrder;
                vehicleDepreciation.Price = double.Parse(model.Price.Replace(",", "").Replace(".", ""));

                int minValue = 0, maxValue = 0;
                string nameLower = model.Name.Trim().ToLower(), patternBelow = @"dưới\D*([0-9]+)",
                    patternAbove = @"trên\D*([0-9]+)",
                    patternFromTo = @"từ\D*([0-9]+)\D*đến\D*([0-9]+)",
                    patternOverTo = @"trên\D*([0-9]+)\D*đến\D*([0-9]+)";

                Regex regex = new Regex(patternBelow);
                Match match = regex.Match(nameLower);

                if (match.Success)
                {
                    int.TryParse(match.Groups[1].Value, out maxValue);
                    if (maxValue > 0)
                    {
                        maxValue -= 1;
                    }
                }
                else
                {
                    regex = new Regex(patternFromTo);
                    match = regex.Match(nameLower);

                    if (match.Success)
                    {
                        int.TryParse(match.Groups[1].Value, out minValue);
                        int.TryParse(match.Groups[2].Value, out maxValue);
                    }
                    else
                    {
                        regex = new Regex(patternOverTo);
                        match = regex.Match(nameLower);

                        if (match.Success)
                        {
                            int.TryParse(match.Groups[1].Value, out minValue);
                            int.TryParse(match.Groups[2].Value, out maxValue);
                            if (minValue > 0 && maxValue > 0)
                            {
                                minValue += 1;
                            }
                        }
                        else
                        {
                            regex = new Regex(patternAbove);
                            match = regex.Match(nameLower);

                            if (match.Success)
                            {
                                int.TryParse(match.Groups[1].Value, out minValue);
                                if (minValue > 0)
                                {
                                    minValue += 1;
                                }
                                maxValue = 1000;
                            }
                        }
                    }
                }

                if (minValue > 0 || maxValue > 0)
                {
                    if (minValue < maxValue || maxValue == 0)
                    {
                        vehicleDepreciation.MinValue = minValue;
                        vehicleDepreciation.MaxValue = maxValue;

                        Tuple<string, string> tuple = await vehicleDepreciation.Update();

                        if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                        {
                            return Json(new
                            {
                                Completed = true,
                                Message = tuple.Item2,
                                Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "VehicleDepreciation", new { Area = "Admin" }))))
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
                    else
                    {
                        return Json(new
                        {
                            Completed = false,
                            Message = "Thời gian sử dụng không hợp lệ, thời gian bắt đầu phải nhỏ hơn thời gian kết thúc. </br> Vui lòng nhập dạng: </br> - Dưới x năm </br> - Từ x đến y năm </br> - Trên x năm đến y năm </br> - Trên x năm"
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = "Thời gian sử dụng không hợp lệ. </br> Vui lòng nhập dạng: </br> - Dưới x năm </br> - Từ x đến y năm </br> - Trên x năm đến y năm </br> - Trên x năm"
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
        public async Task<JsonResult> Erase(int vehicleDepreciationId = 0)
        {
            if (vehicleDepreciationId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            VehicleDepreciation vehicleDepreciation = await VehicleDepreciation.Static_GetById(myPrincipal.UserName, vehicleDepreciationId);

            if (vehicleDepreciation == null || vehicleDepreciation.VehicleDepreciationId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            vehicleDepreciation.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await vehicleDepreciation.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "VehicleDepreciation", new { Area = "Admin" }))))
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