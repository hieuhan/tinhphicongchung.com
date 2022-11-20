using Quartz;
using System;
using System.Threading.Tasks;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;
using tinhphicongchung.com.Models;

namespace tinhphicongchung.com.Services
{
    public class TaskService : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
			try
			{
                if (ConstantHelper.ExecuteTaskServiceCallSchedulingStatus == 1)
                {
                    CacheHelper cacheHelper = new CacheHelper();

                    var hitCounterTask = HitCounter.Static_GetAsync();
                    var menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                    var seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                    var pageTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                    var yearBuildTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);
                    var vehicleTypesTask = VehicleTypes.Static_GetList(ConstantHelper.StatusIdActivated);
                    var vehicleRegistrationYearsTask = VehicleRegistrationYears.Static_GetList(ConstantHelper.StatusIdActivated);
                    var constructionLevelsTask = ConstructionLevels.Static_GetList(ConstantHelper.StatusIdActivated);
                    var landsTask = Lands.Static_GetList(ConstantHelper.StatusIdActivated);
                    var apartmentsTask = Apartments.Static_GetList(ConstantHelper.StatusIdActivated);
                    var apartmentTypesTask = ApartmentTypes.Static_GetList();
                    var districtsTask = Districts.Static_GetListDisplay();

                    await Task.WhenAll(hitCounterTask, menuItemsTask, seosTask,
                        pageTask, yearBuildTask, vehicleTypesTask, vehicleRegistrationYearsTask,
                        constructionLevelsTask, landsTask, apartmentsTask, apartmentTypesTask, districtsTask);


                    CacheVM model = new CacheVM
                    {
                        HitCounter = hitCounterTask.Result,
                        MenuItemsList = menuItemsTask.Result,
                        SeosList = seosTask.Result,
                        Pages = pageTask.Result,
                        YearBuiltsList = yearBuildTask.Result,
                        VehicleTypesList = vehicleTypesTask.Result,
                        VehicleRegistrationYearsList = vehicleRegistrationYearsTask.Result, 
                        ConstructionLevelsList = constructionLevelsTask.Result,
                        LandsList = landsTask.Result,
                        ApartmentsList = apartmentsTask.Result,
                        ApartmentTypesList = apartmentTypesTask.Result,
                        DistrictsList = districtsTask.Result
                    };

                    cacheHelper.Set<CacheVM>("CacheVM", model, ConstantHelper.TaskServiceCacheTimeInSecond);
                }
            }
			catch (Exception ex)
			{
                LogHelper.Error("TaskService > Execute", ex);
			}
        }
    }
}