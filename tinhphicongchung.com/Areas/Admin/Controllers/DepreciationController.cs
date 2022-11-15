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
    public class DepreciationController : Controller
    {
        // GET: Admin/Depreciation

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Depreciation depreciation = new Depreciation
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            byte joinConstructionLevels = 1;
            var statusTask = Status.Static_GetList();
            var depreciationGetPageTask = depreciation.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, joinConstructionLevels, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, depreciationGetPageTask);

            DepreciationVM model = new DepreciationVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                DepreciationList = depreciationGetPageTask.Result != null && depreciationGetPageTask.Result.Item1.IsAny() ? depreciationGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, depreciationGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Depreciation depreciation = new Depreciation
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            byte joinConstructionLevels = 1;
            var statusTask = Status.Static_GetList();
            var depreciationGetPageTask = depreciation.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, joinConstructionLevels, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, depreciationGetPageTask);

            DepreciationVM model = new DepreciationVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                DepreciationList = depreciationGetPageTask.Result != null && depreciationGetPageTask.Result.Item1.IsAny() ? depreciationGetPageTask.Result.Item1 : null,
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

            var statusTask = Status.Static_GetList();
            var constructionLevelsTask = ConstructionLevels.Static_GetList();
            await Task.WhenAll(statusTask, constructionLevelsTask);

            DepreciationEditVM model = new DepreciationEditVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(DepreciationEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Depreciation depreciation = new Depreciation
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    depreciation.Description = model.Description;
                depreciation.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    depreciation.DisplayOrder = model.DisplayOrder;
                depreciation.ConstructionLevelId = model.ConstructionLevelId;
                depreciation.Price = double.Parse(model.Price.Replace(",", "").Replace(".", ""));

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
                    if(maxValue > 0)
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
                            if(minValue > 0 && maxValue > 0)
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

                if(minValue > 0 || maxValue > 0)
                {
                    if (minValue < maxValue || maxValue == 0)
                    {
                        depreciation.MinValue = minValue;
                        depreciation.MaxValue = maxValue;

                        Tuple<string, string> tuple = await depreciation.Insert();

                        if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                        {
                            if (model.AddAnother)
                            {
                                return Json(new
                                {
                                    Completed = true,
                                    Message = tuple.Item2,
                                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-depreciation-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "Depreciation", new { Area = "Admin" }))))
                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new
                                {
                                    Completed = true,
                                    Message = tuple.Item2,
                                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Depreciation", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int depreciationId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (depreciationId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Depreciation depreciation = await Depreciation.Static_GetById(myPrincipal.UserName, depreciationId);

            if (depreciation == null || depreciation.DepreciationId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            var constructionLevelsTask = ConstructionLevels.Static_GetList();
            await Task.WhenAll(statusTask, constructionLevelsTask);

            var model = new DepreciationEditVM
            {
                Name = depreciation.Name,
                Description = depreciation.Description.DefaultIfEmpty(),
                StatusId = depreciation.StatusId,
                DisplayOrder = depreciation.DisplayOrder,
                ConstructionLevelId = depreciation.ConstructionLevelId,
                Price = string.Format("{0:0,0}", depreciation.Price),
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ConstructionLevelsList = constructionLevelsTask != null && constructionLevelsTask.Result.IsAny() ? constructionLevelsTask.Result : new List<ConstructionLevels>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(DepreciationEditVM model)
        {
            if (model.DepreciationId <= 0)
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

                var depreciation = await Depreciation.Static_GetById(myPrincipal.UserName, model.DepreciationId);

                if (depreciation == null || depreciation.DepreciationId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                depreciation.ActBy = myPrincipal.UserName;
                depreciation.Name = model.Name;
                depreciation.Description = model.Description.DefaultIfEmpty();
                depreciation.StatusId = model.StatusId;
                depreciation.DisplayOrder = model.DisplayOrder;
                depreciation.ConstructionLevelId = model.ConstructionLevelId;
                depreciation.Price = double.Parse(model.Price.Replace(",", "").Replace(".", ""));

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
                        depreciation.MinValue = minValue;
                        depreciation.MaxValue = maxValue;

                        Tuple<string, string> tuple = await depreciation.Update();

                        if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                        {
                            return Json(new
                            {
                                Completed = true,
                                Message = tuple.Item2,
                                Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Depreciation", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int depreciationId = 0)
        {
            if (depreciationId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Depreciation depreciation = await Depreciation.Static_GetById(myPrincipal.UserName, depreciationId);

            if (depreciation == null || depreciation.DepreciationId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            depreciation.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await depreciation.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "Depreciation", new { Area = "Admin" }))))
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