using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class FileVM : BaseViewModel
    {
        public string ViewType { get; set; }
        public List<Status> StatusList { get; set; }
        public List<Files> FilesList { get; set; }
    }

    public class FileEditVM : BaseViewModel
    {
        public int FileId { get; set; }

        [Display(Name = "Tên file*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "Loại file")]
        public int FileTypeId { get; set; }

        [Display(Name = "Đường dẫn file")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 1)]
        public string Path { get; set; }

        [Display(Name = "Dung lượng file")]
        public int FileSize { get; set; }

        [Display(Name = "Loại nội dung file")]
        [StringLength(200, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 1)]
        public string ContentType { get; set; }

        [Display(Name = "Thêm tiếp dữ liệu khác")]
        public bool AddAnother { get; set; }
        public List<Status> StatusList { get; set; }
    }
}