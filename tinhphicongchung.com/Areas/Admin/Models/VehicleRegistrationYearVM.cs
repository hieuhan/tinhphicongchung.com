using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class VehicleRegistrationYearVM : BaseViewModel
    {
        public List<Status> StatusList { get; set; }
        public List<VehicleRegistrationYears> VehicleRegistrationYearsList { get; set; }
    }

    public class VehicleRegistrationYearEditVM : BaseViewModel
    {
        public int VehicleRegistrationYearId { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "Năm đăng ký xe")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [Range(2000, 2500, ErrorMessage = "Vui lòng nhập {0} trong khoảng 2000 đến 2500")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public int? Value { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Thêm tiếp dữ liệu khác")]
        public bool AddAnother { get; set; }
        public List<Status> StatusList { get; set; }
    }
}