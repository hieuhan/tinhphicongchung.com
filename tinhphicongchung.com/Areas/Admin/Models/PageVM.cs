using System.ComponentModel.DataAnnotations;
using System.Web;
using tinhphicongchung.com.Areas.Admin.Services.Attributes;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class PageVM : BaseViewModel
    {
        public int PageId { get; set; }

        [Display(Name = "Tên website*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        [RegularExpression(@"[a-zA-Z0-9_\.]+@[a-zA-Z]+\.[a-zA-Z]+(\.[a-zA-Z]+)*", ErrorMessage = "{0} không hợp lệ !")]
        public string Email { get; set; }

        [Display(Name = "Tel")]
        [StringLength(50, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Tel { get; set; }

        [Display(Name = "Fax")]
        [StringLength(50, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Fax { get; set; }

        [Display(Name = "Hotline1")]
        [StringLength(50, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Hotline1 { get; set; }

        [Display(Name = "Hotline2")]
        [StringLength(50, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Hotline2 { get; set; }

        [Display(Name = "Hotline3")]
        [StringLength(50, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Hotline3 { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(500, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Address { get; set; }

        [Display(Name = "Ảnh đại diện")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string ImagePath { get; set; }

        [AllowFileExtensions("jpg,jpeg,png,gif,svg", ErrorMessage = "Vui lòng chọn file ảnh.")]
        [AllowFileSize(FileSize = 20 * 1024 * 1024, ErrorMessage = "Kích thước file cho phép tối đa là 20 MB")]
        public HttpPostedFileBase PostedFile { get; set; }

        [Display(Name = "Tiêu đề SEO")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string MetaTitle { get; set; }

        [Display(Name = "Mô tả SEO")]
        [StringLength(500, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string MetaDescription { get; set; }

        [Display(Name = "Từ khóa SEO")]
        [StringLength(500, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string MetaKeyword { get; set; }

        [Display(Name = "Thẻ H1")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string H1Tag { get; set; }

        [Display(Name = "Canonical")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string CanonicalTag { get; set; }

        [Display(Name = "SeoFooter")]
        //[StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string SeoFooter { get; set; }

        [Display(Name = "Facebook")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Facebook { get; set; }

        [Display(Name = "Twitter")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Twitter { get; set; }

        [Display(Name = "LinkedIn")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string LinkedIn { get; set; }

        [Display(Name = "Youtube")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Youtube { get; set; }
    }
}