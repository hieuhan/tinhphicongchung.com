using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using tinhphicongchung.com.Areas.Admin.Models;
using tinhphicongchung.com.Areas.Admin.Services.Attributes;
using tinhphicongchung.com.Areas.Admin.Services.Extensions;
using tinhphicongchung.com.Areas.Admin.Services.Sercurity;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Controllers
{
    [AuthorizeRole]
    public class ProvinceController : Controller
    {
        private readonly string tableName = "Province";

        // GET: Admin/Province
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Provinces provinces = new Provinces
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var provincesGetPageTask = provinces.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);
            
            await Task.WhenAll(statusTask, provincesGetPageTask);
            
            ProvinceVM model = new ProvinceVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                ProvincesList = provincesGetPageTask.Result != null && provincesGetPageTask.Result.Item1.IsAny() ? provincesGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, provincesGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            ProvinceEditVM model = new ProvinceEditVM
            {
                StatusList = await Status.Static_GetList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(ProvinceEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Provinces provinces = new Provinces
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    provinces.Description = model.Description;
                provinces.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    provinces.DisplayOrder = model.DisplayOrder;

                Tuple<string, string> tuple = await provinces.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    this.ToastrSuccess("Thông báo", tuple.Item2);

                    if (!model.AddAnother)
                    {
                        return Redirect(Url.Action("Index", tableName, new { Area = "Admin" }));
                    }
                }
                else
                {
                    this.ToastrError("Thông báo", tuple.Item2);
                }

                if (model.AddAnother)
                {
                    ModelState.Clear();
                    model.Name = string.Empty;
                    model.Description = string.Empty;
                    model.StatusId = 0;
                    model.DisplayOrder = 0;
                }
            }

            List<Status> statusList = await Status.Static_GetList();

            model.StatusList = statusList.IsAny() ? statusList : new List<Status>();

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int provinceId = 0)
        {
            if (provinceId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Provinces provinces = await Provinces.Static_GetById(myPrincipal.UserName, provinceId);

            if (provinces == null || provinces.ProvinceId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (provinces.BuildIn == 1)
            {
                return Redirect(Url.Action("AccessDenied", "Error", new { Area = "Admin" }));
            }

            List<Status> statusList = await Status.Static_GetList();

            var model = new ProvinceEditVM
            {
                //PreviousPage = SessionHelper.Get<string>(redirectUrl),
                Name = provinces.Name,
                Description = provinces.Description.DefaultIfEmpty(),
                StatusId = provinces.StatusId,
                DisplayOrder = provinces.DisplayOrder,
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProvinceEditVM model)
        {
            if (model.ProvinceId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                var provinces = await Provinces.Static_GetById(myPrincipal.UserName, model.ProvinceId);

                if (provinces == null || provinces.ProvinceId <= 0)
                {
                    return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
                }

                if (provinces.BuildIn == 1)
                {
                    return Redirect(Url.Action("AccessDenied", "Error", new { Area = "Admin" }));
                }

                provinces.ActBy = myPrincipal.UserName;
                provinces.Name = model.Name;
                provinces.Description = model.Description.DefaultIfEmpty();
                provinces.StatusId = model.StatusId;
                provinces.DisplayOrder = model.DisplayOrder;

                Tuple<string, string> tuple = await provinces.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    this.ToastrSuccess("Thông báo", tuple.Item2);

                    return Redirect(Url.Action("Index", tableName, new { Area = "Admin" }));
                }
                else
                {
                    this.ToastrError("Thông báo", tuple.Item2);
                }
            }

            List<Status> statusList = await Status.Static_GetList();

            model.StatusList = statusList.IsAny() ? statusList : new List<Status>();
  
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Erase(int provinceId = 0)
        {
            if (provinceId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Provinces provinces = await Provinces.Static_GetById(myPrincipal.UserName, provinceId);

            if (provinces == null || provinces.ProvinceId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (provinces.BuildIn == 1)
            {
                return Redirect(Url.Action("AccessDenied", "Error", new { Area = "Admin" }));
            }

            provinces.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await provinces.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                this.ToastrSuccess("Thông báo", tuple.Item2);
            }
            else
            {
                this.ToastrError("Thông báo", tuple.Item2);
            }

            return Redirect(
                Url.Action("Index", tableName, new { Area = "Admin" }));
        }
    }
}