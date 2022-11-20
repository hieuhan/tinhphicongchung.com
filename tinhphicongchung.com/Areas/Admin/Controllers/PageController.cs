using System;
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
    public class PageController : Controller
    {
        // GET: Admin/Page

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Pages pages = await Pages.Static_GetById(myPrincipal.UserName, ConstantHelper.DefaultPageId);

            if (pages == null || pages.PageId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            PageVM model = new PageVM();

            model.CopyPropertiesFrom(pages);

            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(PageVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Pages pages = await Pages.Static_GetById(myPrincipal.UserName, model.PageId);

                if (pages == null || pages.PageId <= 0)
                {
                    return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
                }

                pages.CopyPropertiesFrom(model);
                pages.ActBy = myPrincipal.UserName;

                if (string.IsNullOrWhiteSpace(model.ImagePath))
                {
                    string rootDir = Request.PhysicalApplicationPath,
                    filePath = string.Empty;

                    if (model.PostedFile != null)
                    {
                        filePath = FileUploadHelper.SaveFile(model.PostedFile,
                            rootDir, ConstantHelper.MediaPath, true);
                    }

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        pages.ImagePath = filePath.Replace("\\", "/");
                    }
                }

                Tuple<string, string> tuple = await pages.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    this.ToastrSuccess("Thông báo", tuple.Item2);

                    //return Redirect(Url.Action("Index", tableName, new { Area = "Admin" }));
                }
                else
                {
                    this.ToastrError("Thông báo", tuple.Item2);
                }

                //if (tuple.Item1.Equals(CmsConstants.ActionStatusSuccess))
                //{
                //    HttpContext.Cache.Remove(CmsConstants.SharedViewsCacheKey);
                //    this.ToastrSuccess("Thông báo", tuple.Item2);
                //}
                //else
                //{
                //    this.ToastrError("Thông báo", tuple.Item2);
                //}
                //return Redirect(Url.Action("Index", "Pages", new { area = "Admin" }));
            }
            return View(model);
        }
    }
}