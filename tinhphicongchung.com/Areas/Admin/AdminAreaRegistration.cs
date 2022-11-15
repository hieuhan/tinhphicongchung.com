using System.Web.Mvc;

namespace tinhphicongchung.com.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            #region Dashboard

            context.MapRoute(
                "Admin_Dashboard",
                "admin/dashboard.html",
                new { action = "Index", Controller = "Dashboard" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Account

            context.MapRoute(
                "Admin_Login",
                "admin/login.html",
                new { action = "Login", Controller = "Account" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Profile",
                "admin/profile.html",
                new { action = "UserProfile", Controller = "Account" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_ChangePassword",
                "admin/change-password.html",
                new { action = "ChangePassword", Controller = "Account" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Logout",
                "admin/logout.html",
                new { action = "Logout", Controller = "Account" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Role

            context.MapRoute(
                "Admin_Role",
                "admin/role/list.html",
                new { action = "Index", Controller = "Role" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_AddRole",
                "admin/role/add.html",
                new { action = "Add", Controller = "Role" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_EditRole",
                "admin/role/edit/{roleId}.html",
                new { action = "Edit", Controller = "Role", roleId = UrlParameter.Optional },
                new { roleId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_EraseRole",
                "admin/role/del/{roleId}.html",
                new { action = "Erase", Controller = "Role", roleId = UrlParameter.Optional },
                new { roleId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_RoleAction",
                "admin/role/assign/{roleId}.html",
                new { action = "Index", Controller = "RoleAction", roleId = UrlParameter.Optional },
                new { roleId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region User

            context.MapRoute(
                "Admin_User",
                "admin/user.html",
                new { controller = "User", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_AddUser",
                "admin/user/add.html",
                new { controller = "User", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_EditUser",
                "admin/user/edit/{userId}.html",
                new { controller = "User", action = "Edit", userId = UrlParameter.Optional },
                new { userId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_EraseUser",
                "admin/user/del/{userId}.html",
                new { controller = "User", action = "Erase", userId = UrlParameter.Optional },
                new { userId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Action

            context.MapRoute(
                "Admin_Action",
                "admin/action.html",
                new { controller = "Action", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_AddAction",
                "admin/action/add.html",
                new { controller = "Action", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_EditAction",
                "admin/action/edit/{actionId}.html",
                new { controller = "Action", action = "Edit", actionId = UrlParameter.Optional },
                new { actionId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_EraseAction",
                "admin/action/del/{actionId}.html",
                new { controller = "Action", action = "Erase", actionId = UrlParameter.Optional },
                new { actionId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region District

            context.MapRoute(
                "Admin_District",
                "admin/district.html",
                new { controller = "District", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_District_Ajax",
                "ajax/admin/district.html",
                new { controller = "District", action = "BindData" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_AddDistrict",
                "admin/district/add.html",
                new { controller = "District", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_EditDistrict",
                "admin/district/edit/{districtId}.html",
                new { controller = "District", action = "Edit", districtId = UrlParameter.Optional },
                new { districtId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_EraseDistrict",
                "admin/district/del/{districtId}.html",
                new { controller = "District", action = "Erase", districtId = UrlParameter.Optional },
                new { districtId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Wards

            context.MapRoute(
                "Admin_Wards",
                "admin/wards.html",
                new { controller = "Wards", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Wards_Ajax",
                "ajax/admin/wards.html",
                new { controller = "Wards", action = "BindData" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_Wards",
                "admin/wards/add.html",
                new { controller = "Wards", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_Wards",
                "admin/wards/edit/{wardId}.html",
                new { controller = "Wards", action = "Edit", wardId = UrlParameter.Optional },
                new { wardId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_Wards",
                "admin/wards/del/{wardId}.html",
                new { controller = "Wards", action = "Erase", wardId = UrlParameter.Optional },
                new { wardId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Street

            context.MapRoute(
                "Admin_Street",
                "admin/street.html",
                new { controller = "Street", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Street_Ajax",
                "ajax/admin/street.html",
                new { controller = "Street", action = "BindData" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_Street",
                "admin/street/add.html",
                new { controller = "Street", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_Street",
                "admin/street/edit/{streetId}.html",
                new { controller = "Street", action = "Edit", streetId = UrlParameter.Optional },
                new { streetId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_Street",
                "admin/street/del/{streetId}.html",
                new { controller = "Street", action = "Erase", streetId = UrlParameter.Optional },
                new { streetId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Location

            context.MapRoute(
                "Admin_Location",
                "admin/location.html",
                new { controller = "Location", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Location_Ajax",
                "ajax/admin/location.html",
                new { controller = "Location", action = "BindData" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_Location",
                "admin/location/add.html",
                new { controller = "Location", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_Location",
                "admin/location/edit/{locationId}.html",
                new { controller = "Location", action = "Edit", locationId = UrlParameter.Optional },
                new { locationId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_Location",
                "admin/location/del/{locationId}.html",
                new { controller = "Location", action = "Erase", locationId = UrlParameter.Optional },
                new { locationId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region LocationType

            context.MapRoute(
                "Admin_LocationType",
                "admin/locationtype.html",
                new { controller = "LocationType", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_LocationType_Ajax",
                "ajax/admin/locationtype.html",
                new { controller = "LocationType", action = "BindData" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_LocationType",
                "admin/locationtype/add.html",
                new { controller = "LocationType", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_LocationType",
                "admin/locationtype/edit/{locationTypeId}.html",
                new { controller = "LocationType", action = "Edit", locationTypeId = UrlParameter.Optional },
                new { locationTypeId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_LocationType",
                "admin/locationtype/del/{locationTypeId}.html",
                new { controller = "LocationType", action = "Erase", locationTypeId = UrlParameter.Optional },
                new { locationTypeId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region ConstructionLevel

            context.MapRoute(
                "Admin_ConstructionLevel",
                "admin/constructionlevel.html",
                new { controller = "ConstructionLevel", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_ConstructionLevel",
                "admin/constructionlevel/add.html",
                new { controller = "ConstructionLevel", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_ConstructionLevel",
                "admin/constructionlevel/edit/{constructionLevelId}.html",
                new { controller = "ConstructionLevel", action = "Edit", constructionLevelId = UrlParameter.Optional },
                new { constructionLevelId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_ConstructionLevel",
                "admin/constructionlevel/del/{constructionLevelId}.html",
                new { controller = "ConstructionLevel", action = "Erase", constructionLevelId = UrlParameter.Optional },
                new { constructionLevelId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Depreciation

            context.MapRoute(
                "Admin_Depreciation",
                "admin/depreciation.html",
                new { controller = "Depreciation", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Depreciation_Ajax",
                "ajax/admin/depreciation.html",
                new { controller = "Depreciation", action = "BindData" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_Depreciation",
                "admin/depreciation/add.html",
                new { controller = "Depreciation", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_Depreciation",
                "admin/depreciation/edit/{depreciationId}.html",
                new { controller = "Depreciation", action = "Edit", depreciationId = UrlParameter.Optional },
                new { depreciationId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_Depreciation",
                "admin/depreciation/del/{depreciationId}.html",
                new { controller = "Depreciation", action = "Erase", depreciationId = UrlParameter.Optional },
                new { depreciationId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Apartment

            context.MapRoute(
                "Admin_Apartment",
                "admin/apartment.html",
                new { controller = "Apartment", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Apartment_Ajax",
                "ajax/admin/apartment.html",
                new { controller = "Apartment", action = "BindData" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_Apartment",
                "admin/apartment/add.html",
                new { controller = "Apartment", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_Apartment",
                "admin/apartment/edit/{apartmentId}.html",
                new { controller = "Apartment", action = "Edit", apartmentId = UrlParameter.Optional },
                new { apartmentId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_Apartment",
                "admin/apartment/del/{apartmentId}.html",
                new { controller = "Apartment", action = "Erase", apartmentId = UrlParameter.Optional },
                new { apartmentId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Land

            context.MapRoute(
                "Admin_Land",
                "admin/land.html",
                new { controller = "Land", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_Land",
                "admin/land/add.html",
                new { controller = "Land", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_Land",
                "admin/land/edit/{landId}.html",
                new { controller = "Land", action = "Edit", landId = UrlParameter.Optional },
                new { landId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_Land",
                "admin/land/del/{landId}.html",
                new { controller = "Land", action = "Erase", landId = UrlParameter.Optional },
                new { landId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Yearbuilt

            context.MapRoute(
                "Admin_Yearbuilt",
                "admin/yearbuilt.html",
                new { controller = "YearBuilt", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Yearbuilt_Ajax",
                "ajax/admin/yearbuilt.html",
                new { controller = "YearBuilt", action = "BindData" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_Yearbuilt",
                "admin/yearbuilt/add.html",
                new { controller = "YearBuilt", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_Yearbuilt",
                "admin/yearbuilt/edit/{yearBuiltId}.html",
                new { controller = "YearBuilt", action = "Edit", yearBuiltId = UrlParameter.Optional },
                new { yearBuiltId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_Yearbuilt",
                "admin/yearbuilt/del/{yearBuiltId}.html",
                new { controller = "YearBuilt", action = "Erase", yearBuiltId = UrlParameter.Optional },
                new { yearBuiltId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region VehicleType

            context.MapRoute(
                "Admin_VehicleType",
                "admin/vehicletype.html",
                new { controller = "VehicleType", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_VehicleType",
                "admin/vehicletype/add.html",
                new { controller = "VehicleType", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_VehicleType",
                "admin/vehicletype/edit/{vehicleTypeId}.html",
                new { controller = "VehicleType", action = "Edit", vehicleTypeId = UrlParameter.Optional },
                new { vehicleTypeId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_VehicleType",
                "admin/vehicletype/del/{vehicleTypeId}.html",
                new { controller = "VehicleType", action = "Erase", vehicleTypeId = UrlParameter.Optional },
                new { vehicleTypeId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region VehicleDepreciation

            context.MapRoute(
                "Admin_VehicleDepreciation",
                "admin/vehicledepreciation.html",
                new { controller = "VehicleDepreciation", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_VehicleDepreciation",
                "admin/vehicledepreciation/add.html",
                new { controller = "VehicleDepreciation", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_VehicleDepreciation",
                "admin/vehicledepreciation/edit/{vehicleDepreciationId}.html",
                new { controller = "VehicleDepreciation", action = "Edit", vehicleDepreciationId = UrlParameter.Optional },
                new { vehicleDepreciationId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_VehicleDepreciation",
                "admin/vehicledepreciation/del/{vehicleDepreciationId}.html",
                new { controller = "VehicleDepreciation", action = "Erase", vehicleDepreciationId = UrlParameter.Optional },
                new { vehicleDepreciationId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region VehicleRegistrationYear

            context.MapRoute(
                "Admin_VehicleRegistrationYear",
                "admin/vehicleregistrationyear.html",
                new { controller = "VehicleRegistrationYear", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Add_VehicleRegistrationYear",
                "admin/vehicleregistrationyear/add.html",
                new { controller = "VehicleRegistrationYear", action = "Add" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Edit_VehicleRegistrationYear",
                "admin/vehicleregistrationyear/edit/{vehicleRegistrationYearId}.html",
                new { controller = "VehicleRegistrationYear", action = "Edit", vehicleRegistrationYearId = UrlParameter.Optional },
                new { vehicleRegistrationYearId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_Erase_VehicleRegistrationYear",
                "admin/vehicleregistrationyear/del/{vehicleRegistrationYearId}.html",
                new { controller = "VehicleRegistrationYear", action = "Erase", vehicleRegistrationYearId = UrlParameter.Optional },
                new { vehicleRegistrationYearId = @"\d+" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region Page

            context.MapRoute(
                "Admin_Page",
                "admin/page.html",
                new { controller = "Page", action = "Edit" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            #region File

            context.MapRoute(
                "Admin_File",
                "admin/file.html",
                new { controller = "File", action = "Index" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            context.MapRoute(
                "Admin_NotFound",
                "admin/404.html",
                new { controller = "Error", action = "NotFound" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Admin_AccessDenied",
                "admin/403.html",
                new { controller = "Error", action = "AccessDenied" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #region Ajax

            context.MapRoute(
                "Ajax_Admin_GetDistrictsBy",
                "api/admin/district/get.html",
                new { action = "GetDistrictsBy", Controller = "Ajax" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Ajax_Admin_GetWardsBy",
                "api/admin/ward/get.html",
                new { action = "GetWardsBy", Controller = "Ajax" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Ajax_Admin_GetStreetsBy",
                "api/admin/street/get.html",
                new { action = "GetStreetsBy", Controller = "Ajax" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            context.MapRoute(
                "Ajax_Admin_UploadFiles",
                "api/admin/upload.html",
                new { action = "UploadFiles", Controller = "Ajax" },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );

            #endregion

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", Controller = "Dashboard", id = UrlParameter.Optional },
                namespaces: new[] { "tinhphicongchung.com.Areas.Admin.Controllers" }
            );
        }
    }
}