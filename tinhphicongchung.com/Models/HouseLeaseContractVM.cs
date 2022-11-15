using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace tinhphicongchung.com.Models
{
    public class HouseLeaseContractVM : BaseViewModel
    {
        [Display(Name = "Thời gian cho thuê")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public string RentalPeriod { get; set; }

        [Display(Name = "Giá trị Hợp đồng")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string Amount { get; set; }
    }
}