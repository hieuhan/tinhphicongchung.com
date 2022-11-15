using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using tinhphicongchung.com.Areas.Admin.Services.Attributes;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class MenuItemVM : BaseViewModel
    {
        public int MenuId { get; set; }
        public int ParentId { get; set; }
        public List<Menus> MenusList { get; set; }
        public List<MenuItems> ParentMenuItemsList { get; set; }
        public List<MenuItems> MenuItemsList { get; set; }
        public List<Status> StatusList { get; set; }
    }

    public class MenuItemEditVM : BaseViewModel
    {
        public int MenuItemId { get; set; }

        [Display(Name = "Tên Menu item*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Mô tả")]
        [StringLength(250, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "Đường dẫn*")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [StringLength(255, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 1)]
        public string Path { get; set; }

        [Display(Name = "Target")]
        [StringLength(10, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 1)]
        public string Target { get; set; }

        [Display(Name = "Menu")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int MenuId { get; set; }

        [Display(Name = "Menu item cha")]
        public int ParentId { get; set; }

        [AllowHtml]
        [Display(Name = "Icon")]
        [StringLength(1000, ErrorMessage = "{0} bao gồm từ {2} đến {1} ký tự.", MinimumLength = 3)]
        public string IconPath { get; set; }

        [AllowFileExtensions("jpg,jpeg,png,gif,svg", ErrorMessage = "Vui lòng chọn file ảnh.")]
        [AllowFileSize(FileSize = 20 * 1024 * 1024, ErrorMessage = "Kích thước file cho phép tối đa là 20 MB")]
        public HttpPostedFileBase PostedFile { get; set; }

        [Display(Name = "Thứ tự hiển thị")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public int? DisplayOrder { get; set; }

        [Display(Name = "Thêm tiếp dữ liệu khác")]
        public bool AddAnother { get; set; }

        public List<Menus> MenusList { get; set; }
        public List<Status> StatusList { get; set; }
        public List<MenuItems> ParentMenuItemsList { get; set; }

        public List<ObjectJsonVM> TargetsList = new List<ObjectJsonVM>
        {
            new ObjectJsonVM
            {
                Id = 1,
                Name = "_blank"
            },
            new ObjectJsonVM
            {
                Id = 2,
                Name = "_self"
            },
            new ObjectJsonVM
            {
                Id = 3,
                Name = "_parent"
            },
            new ObjectJsonVM
            {
                Id = 4,
                Name = "_top"
            }
        };
    }
}