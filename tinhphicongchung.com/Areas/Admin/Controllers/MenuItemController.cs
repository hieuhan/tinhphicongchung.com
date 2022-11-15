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
    public class MenuItemController : Controller
    {
        // GET: Admin/MenuItem

        [HttpGet]
        public async Task<ActionResult> Index(string keywords = "", int menuId = 0, int parentId = 0, byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            List<Menus> menusList = await Menus.Static_GetList();

            if(menuId <= 0)
            {
                if (menusList.IsAny())
                {
                    menuId = menusList[0].MenuId;
                }
            }

            MenuItems menuItem = new MenuItems
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                MenuId = menuId,
                ParentId = parentId,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();
            
            Task<List<MenuItems>> parentMenuItemsTask = null;
            var menuItemsGetPageTask = menuItem.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0,page > 0 ? page - 1 : page, pageSize);

            if(menuId > 0)
            {
                parentMenuItemsTask = MenuItems.Static_GetParentList(menuId);
                
                await Task.WhenAll(statusTask, parentMenuItemsTask, menuItemsGetPageTask);
            }
            else
            {
                await Task.WhenAll(statusTask, menuItemsGetPageTask);
            }

            MenuItemVM model = new MenuItemVM
            {
                MenuId = menuId,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                MenusList = menusList.IsAny() ? menusList : new List<Menus>(),
                ParentMenuItemsList = parentMenuItemsTask != null && parentMenuItemsTask.Result.IsAny() ? parentMenuItemsTask.Result : new List<MenuItems>(),
                MenuItemsList = menuItemsGetPageTask != null && menuItemsGetPageTask.Result != null && menuItemsGetPageTask.Result.Item1.IsAny() ? menuItemsGetPageTask.Result.Item1 : new List<MenuItems>(),
                Pagination = new PaginationVM(page, pageSize, menuItemsGetPageTask.Result.Item2)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> BindData(string keywords = "", int menuId = 0, int parentId = 0, byte statusId = 0, int page = 0, int pageSize = 200)
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            List<Menus> menusList = await Menus.Static_GetList();

            if (menuId <= 0)
            {
                if (menusList.IsAny())
                {
                    menuId = menusList[0].MenuId;
                }
            }

            MenuItems menuItem = new MenuItems
            {
                ActBy = myPrincipal.UserName,
                Name = keywords,
                MenuId = menuId,
                ParentId = parentId,
                StatusId = statusId
            };

            var statusTask = Status.Static_GetList();

            Task<List<MenuItems>> parentMenuItemsTask = null;
            var menuItemsGetPageTask = menuItem.GetPage(DateTime.MinValue, DateTime.MinValue, 0, 0, page > 0 ? page - 1 : page, pageSize);

            if (menuId > 0)
            {
                parentMenuItemsTask = MenuItems.Static_GetParentList(menuId);

                await Task.WhenAll(statusTask, parentMenuItemsTask, menuItemsGetPageTask);
            }
            else
            {
                await Task.WhenAll(statusTask, menuItemsGetPageTask);
            }

            MenuItemVM model = new MenuItemVM
            {
                MenuId = menuId,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                MenusList = menusList.IsAny() ? menusList : new List<Menus>(),
                ParentMenuItemsList = parentMenuItemsTask != null && parentMenuItemsTask.Result.IsAny() ? parentMenuItemsTask.Result : new List<MenuItems>(),
                MenuItemsList = menuItemsGetPageTask != null && menuItemsGetPageTask.Result != null && menuItemsGetPageTask.Result.Item1.IsAny() ? menuItemsGetPageTask.Result.Item1 : new List<MenuItems>(),
                Pagination = new PaginationVM(page, pageSize, menuItemsGetPageTask.Result.Item2)
            };

            return PartialView(model);
        }

        [HttpGet]
        public async Task<ActionResult> Add(int menuId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (menuId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            var menusTask = Menus.Static_GetList();
            var parentMenuItemsTask = MenuItems.Static_GetParentList(menuId);

            await Task.WhenAll(statusTask, parentMenuItemsTask, menusTask);

            MenuItemEditVM model = new MenuItemEditVM
            {
                MenuId = menuId,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                MenusList = menusTask != null && menusTask.Result.IsAny() ? menusTask.Result : new List<Menus>(),
                ParentMenuItemsList = parentMenuItemsTask != null && parentMenuItemsTask.Result.IsAny() ? parentMenuItemsTask.Result : new List<MenuItems>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Add(MenuItemEditVM model)
        {
            if (model.MenuId <= 0)
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

                MenuItems menuItems = new MenuItems
                {
                    ActBy = myPrincipal.UserName,
                    Name = model.Name,
                    Path = model.Path
                };

                if (!string.IsNullOrWhiteSpace(model.Description))
                    menuItems.Description = model.Description;
                menuItems.MenuId = model.MenuId;
                menuItems.ParentId = model.ParentId;
                if (!string.IsNullOrWhiteSpace(model.Target))
                    menuItems.Target = model.Target;
                if (!string.IsNullOrWhiteSpace(model.IconPath))
                    menuItems.IconPath = model.IconPath;
                menuItems.StatusId = model.StatusId;
                if (model.DisplayOrder.HasValue)
                    menuItems.DisplayOrder = model.DisplayOrder;

                if (string.IsNullOrWhiteSpace(model.IconPath))
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
                        menuItems.IconPath = filePath.Replace("\\", "/");
                    }
                }

                Tuple<string, string> tuple = await menuItems.Insert();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    if (model.AddAnother)
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('#add-menuitem-form').find('input[type=\"text\"]').val('');app.bindTableData('{0}')", Url.Action("BindData", "MenuItem", new { Area = "Admin" }))))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = true,
                            Message = tuple.Item2,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "MenuItem", new { Area = "Admin" }))))
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
        public async Task<ActionResult> Edit(int menuItemId = 0)
        {
            if (!Request.IsAjaxRequest())
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            if (menuItemId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            MenuItems menuItems = await MenuItems.Static_GetById(myPrincipal.UserName, menuItemId);

            if (menuItems == null || menuItems.MenuItemId <= 0)
            {
                return Redirect(Url.Action("NotFound", "Error", new { Area = "Admin" }));
            }

            var statusTask = Status.Static_GetList();
            var menusTask = Menus.Static_GetList();
            var parentMenuItemsTask = MenuItems.Static_GetParentList(menuItems.MenuId);

            await Task.WhenAll(statusTask, parentMenuItemsTask, menusTask);

            var model = new MenuItemEditVM
            {
                Name = menuItems.Name,
                Description = menuItems.Description.DefaultIfEmpty(),
                MenuId = menuItems.MenuId,
                ParentId = menuItems.ParentId,
                Target = menuItems.Target.DefaultIfEmpty(),
                Path = menuItems.Path,
                IconPath = menuItems.IconPath.DefaultIfEmpty(),
                StatusId = menuItems.StatusId,
                DisplayOrder = menuItems.DisplayOrder,
                StatusList = statusTask != null && statusTask.Result.IsAny() ? statusTask.Result : new List<Status>(),
                MenusList = menusTask != null && menusTask.Result.IsAny() ? menusTask.Result : new List<Menus>(),
                ParentMenuItemsList = parentMenuItemsTask != null && parentMenuItemsTask.Result.IsAny() ? parentMenuItemsTask.Result : new List<MenuItems>()
            };

            return PartialView(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Edit(MenuItemEditVM model)
        {
            if (model.MenuItemId <= 0)
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

                MenuItems menuItems = await MenuItems.Static_GetById(myPrincipal.UserName, model.MenuItemId);

                if (menuItems == null || menuItems.MenuItemId <= 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(new
                    {
                        Completed = false,
                        //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                    }, JsonRequestBehavior.AllowGet);
                }

                menuItems.ActBy = myPrincipal.UserName;
                menuItems.Name = model.Name;
                menuItems.Description = model.Description.DefaultIfEmpty();
                menuItems.MenuId = model.MenuId;
                menuItems.ParentId = model.ParentId;
                menuItems.Target = model.Target.DefaultIfEmpty();
                menuItems.Path = model.Path;
                menuItems.IconPath = model.IconPath.DefaultIfEmpty();
                menuItems.StatusId = model.StatusId;
                menuItems.DisplayOrder = model.DisplayOrder;

                if (string.IsNullOrWhiteSpace(model.IconPath))
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
                        menuItems.IconPath = filePath.Replace("\\", "/");
                    }
                }

                Tuple<string, string> tuple = await menuItems.Update();

                if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Message = tuple.Item2,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_editForm');app.bindTableData('{0}')", Url.Action("BindData", "MenuItem", new { Area = "Admin" }))))
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
        public async Task<JsonResult> Erase(int menuItemId = 0)
        {
            if (menuItemId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            MenuItems menuItems = await MenuItems.Static_GetById(myPrincipal.UserName, menuItemId);

            if (menuItems == null || menuItems.MenuItemId <= 0)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new
                {
                    Completed = false,
                    //ReturnUrl = Url.Action("NotFound", "Error", new { Area = "Admin" })
                }, JsonRequestBehavior.AllowGet);
            }

            menuItems.ActBy = myPrincipal.UserName;
            Tuple<string, string> tuple = await menuItems.Delete();

            if (tuple != null && !string.IsNullOrWhiteSpace(tuple.Item1) && tuple.Item1.Equals(ConstantHelper.ActionStatusSuccess))
            {
                return Json(new
                {
                    Completed = true,
                    Message = tuple.Item2,
                    Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("app.closeDialog('#_message');app.bindTableData('{0}')", Url.Action("BindData", "MenuItem", new { Area = "Admin" }))))
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