using System.Web.Mvc;
using System.Web.Routing;

namespace tinhphicongchung.com
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute(
                "LoanContract",
                "hop-dong-vay-tien.html",
                new { controller = "NotarizationFee", action = "LoanContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "ContractAmendmentsAndSupplements",
                "hop-dong-sua-doi-bo-sung-thay-doi-gia-tri-tai-san.html",
                new { controller = "NotarizationFee", action = "ContractAmendmentsAndSupplements" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "EconomicCommercialInvestmentBusinessContract",
                "hop-dong-kinh-te-thuong-mai-dau-tu-kinh-doanh.html",
                new { controller = "NotarizationFee", action = "EconomicCommercialInvestmentBusinessContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "HouseLeaseContract",
                "hop-dong-thue-nha.html",
                new { controller = "NotarizationFee", action = "HouseLeaseContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "MortgageContract",
                "hop-dong-the-chap.html",
                new { controller = "NotarizationFee", action = "MortgageContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "DepositContract",
                "hop-dong-dat-coc.html",
                new { controller = "NotarizationFee", action = "DepositContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "CapitalContributionContract",
                "hop-dong-gop-von-bang-tien.html",
                new { controller = "NotarizationFee", action = "CapitalContributionContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "ContractCapitalContributionByApartment",
                "hop-dong-gop-von-bang-can-ho-chung-cu.html",
                new { controller = "NotarizationFee", action = "ContractCapitalContributionByApartment" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "PurchaseAndSaleApartmentContract",
                "hop-dong-mua-ban-can-ho-chung-cu.html",
                new { controller = "NotarizationFee", action = "PurchaseAndSaleApartmentContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "ContractDonationApartment",
                "hop-dong-tang-cho-can-ho-chung-cu.html",
                new { controller = "NotarizationFee", action = "ContractDonationApartment" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "DocumentDivideInheritanceApartment",
                "van-ban-phan-chia-di-san-thua-ke-can-ho-chung-cu.html",
                new { controller = "NotarizationFee", action = "DocumentDivideInheritanceApartment" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "LandUseRightTransferContract",
                "hop-dong-chuyen-nhuong-quyen-su-dung-dat.html",
                new { controller = "NotarizationFee", action = "LandUseRightTransferContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "ContractSaleHouseAndTransferLandUseRight",
                "hop-dong-mua-ban-nha-va-chuyen-nhuong-quyen-su-dung-dat.html",
                new { controller = "NotarizationFee", action = "ContractSaleHouseAndTransferLandUseRight" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "ContractDonationHouseLand",
                "hop-dong-tang-cho-nha-dat.html",
                new { controller = "NotarizationFee", action = "ContractDonationHouseLand" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "ContractDonationLandUseRight",
                "hop-dong-tang-cho-quyen-su-dung-dat.html",
                new { controller = "NotarizationFee", action = "ContractDonationLandUseRight" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "HouseLandSeparationContract",
                "hop-dong-chia-tach-nha-dat.html",
                new { controller = "NotarizationFee", action = "HouseLandSeparationContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "ContractDivisionLandUseRight",
                "hop-dong-chia-tach-dat.html",
                new { controller = "NotarizationFee", action = "ContractDivisionLandUseRight" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "ContractCapitalContributionByHouseLand",
                "hop-dong-gop-von-bang-nha-dat.html",
                new { controller = "NotarizationFee", action = "ContractCapitalContributionByHouseLand" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "ContractCapitalContributionByLandUseRight",
                "hop-dong-gop-von-bang-quyen-su-dung-dat.html",
                new { controller = "NotarizationFee", action = "ContractCapitalContributionByLandUseRight" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "DocumentDivisionInheritanceLandUseRight",
                "van-ban-phan-chia-di-san-thua-ke-quyen-su-dung-dat.html",
                new { controller = "NotarizationFee", action = "DocumentDivisionInheritanceLandUseRight" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "DocumentDivisionInheritanceHouseLand",
                "van-ban-phan-chia-di-san-thua-ke-bang-nha-dat.html",
                new { controller = "NotarizationFee", action = "DocumentDivisionInheritanceHouseLand" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            //routes.MapRoute(
            //    "OtherServices",
            //    "dich-vu-khac.html",
            //    new { controller = "Home", action = "OtherServices" },
            //    namespaces: new[] { "tinhphicongchung.com.Controllers" }
            //);

            //routes.MapRoute(
            //    "Certificate",
            //    "giay-chung-nhan.html",
            //    new { controller = "Home", action = "Certificate" },
            //    namespaces: new[] { "tinhphicongchung.com.Controllers" }
            //);

            //routes.MapRoute(
            //    "UserManual",
            //    "huong-dan-su-dung-tinh-phi-cong-chung.html",
            //    new { controller = "Home", action = "UserManual" },
            //    namespaces: new[] { "tinhphicongchung.com.Controllers" }
            //);

            routes.MapRoute(
                "VehicleSalesContract",
                "hop-dong-mua-ban-xe.html",
                new { controller = "NotarizationFee", action = "VehicleSalesContract" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "NotFound",
                "404.html",
                new { controller = "Home", action = "NotFound" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            #region Ajax

            routes.MapRoute(
                "Ajax_GetDistrictsBy",
                "api/district/get.html",
                new { controller = "Ajax", action = "GetDistrictsBy", },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "Ajax_GetWardsBy",
                "api/ward/get.html",
                new { controller = "Ajax", action = "GetWardsBy", },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "Ajax_GetStreetsBy",
                "api/street/get.html",
                new { controller = "Ajax", action = "GetStreetsBy", },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "Ajax_GetLocationsBy",
                "api/location/get.html",
                new { controller = "Ajax", action = "GetLocationsBy", },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.MapRoute(
                "Ajax_GetLandTypessBy",
                "api/landtypes/get.html",
                new { controller = "Ajax", action = "GetLandTypesBy", },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            #endregion

            routes.MapRoute(
                "ArticleDetail",
                "{ArticleUrl}.html",
                new { controller = "Article", action = "Detail" },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "tinhphicongchung.com.Controllers" }
            );
        }
    }
}
