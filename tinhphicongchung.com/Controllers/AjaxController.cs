﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using tinhphicongchung.com.Areas.Admin.Models;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Controllers
{
    public class AjaxController : Controller
    {
        // GET: Ajax
        public async Task<JsonResult> GetDistrictsBy(int provinceId = 0)
        {
            List<ObjectJsonVM> resultVar = new List<ObjectJsonVM>();

            if (provinceId > 0)
            {
                var districtsList = await Districts.Static_GetListByProvince(provinceId, ConstantHelper.StatusIdActivated);
                if (districtsList.IsAny())
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
                var wardsList = await Wards.Static_GetListByDistrict(districtId, ConstantHelper.StatusIdActivated);
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
                var streetsList = await Streets.Static_GetListByWard(wardId, ConstantHelper.StatusIdActivated);
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

        public async Task<JsonResult> GetLocationsBy(byte landTypeId = 0, int streetId = 0)
        {
            List<ObjectJsonVM> resultVar = new List<ObjectJsonVM>();

            if (landTypeId > 0 && streetId > 0)
            {
                var locationTypesList = await LocationTypes.Static_GetBy(landTypeId, streetId);
                if (locationTypesList.IsAny())
                {
                    resultVar.AddRange(locationTypesList.Select(x => new ObjectJsonVM()
                    {
                        Id = x.LocationId,
                        Name = x.Description
                    }));
                }
            }

            return Json(resultVar, JsonRequestBehavior.AllowGet);
        }
    }
}