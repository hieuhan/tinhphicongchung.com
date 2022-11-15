using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class WardVM : BaseViewModel
    {
        public int DistrictId { get; set; }
        public List<Status> StatusList { get; set; }
        public List<Districts> DistrictsList { get; set; }
        public List<Wards> WardsList { get; set; }
    }

    public class WardEditVM : BaseViewModel
    {
        public int WardId { get; set; }

        public string Prefix { get; set; }

        [Display(Name = "Tên Phường / Xã*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 1)]
        public string Description { get; set; }

        [Display(Name = "Quận / Huyện")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int DistrictId { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Thêm tiếp dữ liệu khác")]
        public bool AddAnother { get; set; }
        public List<Status> StatusList { get; set; }
        public List<Districts> DistrictsList { get; set; }
    }
}