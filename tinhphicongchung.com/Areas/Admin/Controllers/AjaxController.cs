using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using tinhphicongchung.com.Areas.Admin.Models;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Controllers
{
    //[AuthorizeRole]
    public class AjaxController : Controller
    {
        // GET: Admin/Ajax
        public async Task<JsonResult> GetDistrictsBy(int provinceId = 0)
        {
            List<ObjectJsonVM> resultVar = new List<ObjectJsonVM>();

            if (provinceId > 0)
            {
                var districtsList = await Districts.Static_GetListByProvince(provinceId);
                if(districtsList.IsAny())
                {
                    resultVar.AddRange(districtsList.Select(x => new ObjectJsonVM()
                    {
                        Id = x.DistrictId,
                        Name = x.Name
                    }));
                }    
            }

            return Json(resultVar, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetWardsBy(int districtId = 0)
        {
            List<ObjectJsonVM> resultVar = new List<ObjectJsonVM>();

            if (districtId > 0)
            {
                var wardsList = await Wards.Static_GetListByDistrict(districtId);
                if (wardsList.IsAny())
                {
                    resultVar.AddRange(wardsList.Select(x => new ObjectJsonVM()
                    {
                        Id = x.WardId,
                        Name = x.Name
                    }));
                }
            }

            return Json(resultVar, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetStreetsBy(int wardId = 0)
        {
            List<ObjectJsonVM> resultVar = new List<ObjectJsonVM>();

            if (wardId > 0)
            {
                var streetsList = await Streets.Static_GetListByWard(wardId);
                if (streetsList.IsAny())
                {
                    resultVar.AddRange(streetsList.Select(x => new ObjectJsonVM()
                    {
                        Id = x.StreetId,
                        Name = x.Name
                    }));
                }
            }

            return Json(resultVar, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UploadFiles(IEnumerable<HttpPostedFileBase> files)
        {
            if (files.IsAny())
            {
                string rootDir = Request.PhysicalApplicationPath,
                    filePath = string.Empty;
                Files file = new Files();
                List<Task<Tuple<string, string>>> taskFiles = new List<Task<Tuple<string, string>>>();

                foreach (var postedFile in files)
                {
                    if (postedFile.ContentType.StartsWith("image/"))
                    {
                        filePath = FileUploadHelper.SaveFile(postedFile,
                            rootDir, ConstantHelper.MediaPath, true);

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            filePath = filePath.Replace("\\", "/");
                            file = new Files
                            {
                                Name = Path.GetFileNameWithoutExtension(postedFile.FileName),
                                Path = filePath,
                                FileTypeId = 1,
                                FileSize = postedFile.ContentLength,
                                ContentType = postedFile.ContentType,
                                StatusId = ConstantHelper.StatusIdActivated
                            };

                            taskFiles.Add(file.Insert());
                        }
                    }
                }

                if (taskFiles.IsAny())
                {
                    await Task.WhenAll(taskFiles);
                }
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }
    }
}