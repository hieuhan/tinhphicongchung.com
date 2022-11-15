using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class ArticleVM : BaseViewModel
    {
        public List<ReviewStatus> ReviewStatusList { get; set; }
        public List<Articles> ArticlesList { get; set; }
    }

    public class ArticleEditVM : BaseViewModel
    {
        public int ArticleId { get; set; }

        [Display(Name = "Tiều đề")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(500, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Title { get; set; }

        [Display(Name = "Trích dẫn")]
        [StringLength(2000, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Summary { get; set; }

        [Display(Name = "Nội dung")]
        public string ArticleContent { get; set; }

        [Display(Name = "Url")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string ArticleUrl { get; set; }

        [Display(Name = "Ảnh bài viết")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string ImagePath { get; set; }

        [Display(Name = "Tiêu đề SEO")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string MetaTitle { get; set; }

        [Display(Name = "Mô tả SEO")]
        [StringLength(500, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string MetaDescription { get; set; }

        [Display(Name = "Từ khóa SEO")]
        [StringLength(500, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string MetaKeyword { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Thêm tiếp dữ liệu khác")]
        public bool AddAnother { get; set; }

        [Display(Name = "Xóa ảnh")]
        public bool RemoveImage { get; set; }
        public List<ReviewStatus> ReviewStatusList { get; set; }
    }
}