using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using tinhphicongchung.com.Areas.Admin.Models;
using tinhphicongchung.com.Areas.Admin.Services.Extensions;
using tinhphicongchung.com.Areas.Admin.Services.Sercurity;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Controllers
{
    public class SharedController : Controller
    {
        // GET: Admin/Shared

        [ChildActionOnly]
        public PartialViewResult PartialHeader()
        {
            MyPrincipal myPrincipal = AppExtensions.GetCurrentUser();

            List<Actions> actionsListByUser = Actions.Static_GetByUser(myPrincipal.UserName, myPrincipal.UserId);

            var model = new HeaderVM
            {
                ActionsList = new List<Actions>(),
                ParentActionsList = new List<Actions>()
            };

            if (actionsListByUser.IsAny())
            {
                foreach (var item in actionsListByUser)
                {
                    if (item.ParentId == 0)
                    {
                        model.ParentActionsList.Add(item);
                    }
                    else model.ActionsList.Add(item);
                }
            }

            return PartialView(model);
        }
    }
}