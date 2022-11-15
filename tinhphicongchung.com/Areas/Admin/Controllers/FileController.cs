using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FileController : Controller
    {
        // GET: Admin/File

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", string viewType = "", byte statusId = 0, int page = 0, int pageSize = 50)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Files files = new Files
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var filesGetPageTask = files.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, filesGetPageTask);

            FileVM model = new FileVM
            {
                ViewType = viewType,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                FilesList = filesGetPageTask.Result != null && filesGetPageTask.Result.Item1.IsAny() ? filesGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, filesGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", string viewType = "", byte statusId = 0, int page = 0, int pageSize = 50)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Files files = new Files
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var filesGetPageTask = files.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, filesGetPageTask);

            FileVM model = new FileVM
            {
                ViewType = viewType,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                FilesList = filesGetPageTask.Result != null && filesGetPageTask.Result.Item1.IsAny() ? filesGetPageTask.Result.Item1 : null,
                Pagination = new PaginationVM(page, pageSize, filesGetPageTask.Result.Item2)
            };

            return PartialView(model);
        }
    }
}