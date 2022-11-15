using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class LandVM : BaseViewModel
    {
        public int ConstructionLevelId { get; set; }
        public List<Status> StatusList { get; set; }
        public List<ConstructionLevels> ConstructionLevelsList { get; set; }
        public List<Lands> LandsList { get; set; }
    }

    public class LandEditVM : BaseViewModel
    {
        public int LandId { get; set; }

        [Display(Name = "Tên công trình*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Description { get; set; }
        public int ConstructionLevelId { get; set; }

        [Display(Name = "Giá (Đ/M2)")]
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
        public List<ConstructionLevels> ConstructionLevelsList { get; set; }
    }
}