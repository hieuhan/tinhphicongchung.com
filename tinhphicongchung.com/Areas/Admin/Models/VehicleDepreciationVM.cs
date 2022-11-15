using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class VehicleDepreciationVM : BaseViewModel
    {
        public List<Status> StatusList { get; set; }
        public List<VehicleDepreciation> VehicleDepreciationList { get; set; }
    }

    public class VehicleDepreciationEditVM : BaseViewModel
    {
        public int VehicleDepreciationId { get; set; }

        [Display(Name = "Thời gian sử dụng*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "Khấu hao (%)")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [Range(1, 99, ErrorMessage = "Vui lòng nhập {0} trong khoảng 1 đến 99")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public string Price { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Thêm tiếp dữ liệu khác")]
        public bool AddAnother { get; set; }
        public List<Status> StatusList { get; set; }
    }
}