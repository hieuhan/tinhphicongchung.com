using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class StreetVM : BaseViewModel
    {
        public int DistrictId { get; set; }
        public int WardId { get; set; }
        public List<Status> StatusList { get; set; }
        public List<Districts> DistrictsList { get; set; }
        public List<Wards> WardsList { get; set; }
        public List<Streets> StreetsList { get; set; }
    }

    public class StreetEditVM : BaseViewModel
    {
        public int StreetId { get; set; }

        [Display(Name = "Tên Đường phố*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 1)]
        public string Name { get; set; }

        public string Prefix { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 1)]
        public string Description { get; set; }

        [Display(Name = "Quận / Huyện")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int DistrictId { get; set; }

        [Display(Name = "Phường / Xã")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int WardId { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Thêm tiếp dữ liệu khác")]
        public bool AddAnother { get; set; }
        public List<Status> StatusList { get; set; }
        public List<Districts> DistrictsList { get; set; }
        public List<Wards> WardsList { get; set; }
    }
}