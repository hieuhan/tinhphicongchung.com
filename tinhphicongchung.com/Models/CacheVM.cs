using System.Collections.Generic;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Models
{
    public class CacheVM
    {
        public HitCounter HitCounter { get; set; }
        public Pages Pages { get; set; }
        public List<Seos> SeosList { get; set; }
        public List<MenuItems> MenuItemsList { get; set; }
        public List<YearBuilts> YearBuiltsList { get; set; }
        public List<VehicleTypes> VehicleTypesList { get; set; }
        public List<VehicleRegistrationYears> VehicleRegistrationYearsList { get; set; }
        public List<ConstructionLevels> ConstructionLevelsList { get; set; }
        public List<Lands> LandsList { get; set; }
        public List<LandTypes> LandTypesList { get; set; }
        public List<Apartments> ApartmentsList { get; set; }
        public List<ApartmentTypes> ApartmentTypesList { get; set; }
        public List<Districts> DistrictsList { get; set; }
    }
}