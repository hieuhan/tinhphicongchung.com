using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Models
{
    public class VehicleSalesContractVM : BaseViewModel
    {
        [Display(Name = "Năm")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int Year { get; set; }

        [Display(Name = "Tháng")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int Month { get; set; }

        [Display(Name = "Ngày")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int Day { get; set; }

        [Display(Name = "Loại xe")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int VehicleTypeId { get; set; }

        [Display(Name = "Giá xe mới (VNĐ)")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        
        public string Price { get; set; }
        public List<ObjectJsonVM> DaysList { get; set; }
        public List<ObjectJsonVM> MonthsList { get; set; }
        public List<VehicleTypes> VehicleTypesList { get; set; }
        public List<VehicleRegistrationYears> VehicleRegistrationYearsList { get; set; }
    }
}