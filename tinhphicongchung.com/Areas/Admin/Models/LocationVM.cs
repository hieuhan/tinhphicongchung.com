using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class LocationVM : BaseViewModel
    {
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public int StreetId { get; set; }
        public int LocationTypeId { get; set; }
        public byte LandTypeId { get; set; }
        public List<Status> StatusList { get; set; }
        public List<LocationTypes> LocationTypesList { get; set; }
        public List<LandTypes> LandTypesList { get; set; }
        public List<Districts> DistrictsList { get; set; }
        public List<Wards> WardsList { get; set; }
        public List<Streets> StreetsList { get; set; }
        public List<Locations> LocationsList { get; set; }
    }

    public class LocationEditVM : BaseViewModel
    {
        public int LocationId { get; set; }

        [Display(Name = "Tên vị trí (Đoạn Đường)*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "Quận / Huyện")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int DistrictId { get; set; }

        [Display(Name = "Phường / Xã")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int WardId { get; set; }

        [Display(Name = "Đường / Phố")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int StreetId { get; set; }

        public int LocationTypeId { get; set; }

        public byte LandTypeId { get; set; }

        [Display(Name = "Đơn Giá (đồng/m2)")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [MaxLength(17, ErrorMessage = "Vui lòng nhập {0} hợp lệ, không vượt quá 1000 tỷ VNĐ")]
        public string Price { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Thêm tiếp dữ liệu khác")]
        public bool AddAnother { get; set; }
        public List<Status> StatusList { get; set; }
        public List<LocationTypes> LocationTypesList { get; set; }
        public List<LandTypes> LandTypesList { get; set; }
        public List<Districts> DistrictsList { get; set; }
        public List<Wards> WardsList { get; set; }
        public List<Streets> StreetsList { get; set; }
    }
}