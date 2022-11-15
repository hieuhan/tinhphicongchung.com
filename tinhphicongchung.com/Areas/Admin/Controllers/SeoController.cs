using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using tinhphicongchung.com.Areas.Admin.Models;
using tinhphicongchung.com.Areas.Admin.Services.Attributes;
using tinhphicongchung.com.Areas.Admin.Services.Extensions;
using tinhphicongchung.com.Areas.Admin.Services.Sercurity;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Controllers
{
    [AuthorizeRole]
    public class SeoController : Controller
    {
        // GET: Admin/Seo

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Seos seos = new Seos
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var seosGetPageTask = seos.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, seosGetPageTask);

            SeoVM model = new SeoVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                SeosList = seosGetPageTask.Result != null && seosGetPageTask.Result.Item1.IsAny() ? seosGetPageTask.Result.Item1 : new List<Seos>(),
                Pagination = new PaginationVM(page, pageSize, seosGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Seos seos = new Seos
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            var seosGetPageTask = seos.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            await Task.WhenAll(statusTask, seosGetPageTask);

            SeoVM model = new SeoVM
            {
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                SeosList = seosGetPageTask.Result != null && seosGetPageTask.Result.Item1.IsAny() ? seosGetPageTask.Result.Item1 : new List<Seos>(),
                Pagination = new PaginationVM(page, pageSize, seosGetPageTask.Result.Item2)
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

            SeoEditVM model = new SeoEditVM
            {
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(SeoEditVM model)
        {
            if (ModelState.IsValid)
            {
                MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

                Seos seos = new Seos
                {
                    ActBy = myPrincipal.UserName
                };

                seos.CopyPropertiesFrom(model);

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
                        seos.ImagePath = filePath.Replace("\\", "/");
                    }
                }

                Tuple<string, string> tuple = await seos.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-seo-form').find('input[type=text]').val('');app.ckeditorClearValue('Description');app.ckeditorClearValue('SeoFooter');app.bindTableData('{0}')", Url.Action("BindData", "Seo", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Seo", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int seoId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (seoId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Seos seos = await Seos.Static_GetById(myPrincipal.UserName, seoId);

            if (seos == null || seos.SeoId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            List<Status> statusList = await Status.Static_GetList();

            var model = new SeoEditVM
            {
                StatusList = statusList.IsAny() ? statusList : new List<Status>()
            };

            model.CopyPropertiesFrom(seos);

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(SeoEditVM model)
        {
            if (model.SeoId <= 0)
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

                Seos seos = await Seos.Static_GetById(myPrincipal.UserName, model.SeoId);

                if (seos == null || seos.SeoId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                seos.CopyPropertiesFrom(model);
                seos.ActBy = myPrincipal.UserName;

                if (!model.RemoveImage)
                {
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
                            seos.ImagePath = filePath.Replace("\\", "/");
                        }
                    }
                }
                else seos.ImagePath = string.Empty;

                Tuple<string, string> tuple = await seos.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "Seo", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int seoId = 0)
        {
            if (seoId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            Seos seos = await Seos.Static_GetById(myPrincipal.UserName, seoId);

            if (seos == null || seos.SeoId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            seos.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await seos.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "Seo", new { Area = "Admin" }))))
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