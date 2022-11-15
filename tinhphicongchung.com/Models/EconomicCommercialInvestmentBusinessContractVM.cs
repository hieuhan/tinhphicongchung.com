using System.ComponentModel.DataAnnotations;

namespace tinhphicongchung.com.Models
{
    public class EconomicCommercialInvestmentBusinessContractVM : BaseViewModel
    {
        [Display(Name = "Giá trị Hợp đồng")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [MaxLength(17, ErrorMessage = "Vui lòng nhập {0} hợp lệ, không vượt quá 1000 tỷ VNĐ")]
        public string Amount { get; set; }
    }
}