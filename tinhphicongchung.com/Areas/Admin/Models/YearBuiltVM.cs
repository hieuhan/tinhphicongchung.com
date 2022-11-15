using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class YearBuiltVM : BaseViewModel
    {
        public List<Status> StatusList { get; set; }
        public List<YearBuilts> YearBuiltsList { get; set; }
    }

    public class YearBuiltEditVM : BaseViewModel
    {
        public int YearBuiltId { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "Năm xây dựng")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [Range(1950, 2500, ErrorMessage = "Vui lòng nhập {0} trong khoảng 1950 đến 2500")]
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