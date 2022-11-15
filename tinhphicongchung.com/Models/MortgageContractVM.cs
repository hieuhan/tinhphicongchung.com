using System.ComponentModel.DataAnnotations;

namespace tinhphicongchung.com.Models
{
    public class MortgageContractVM : BaseViewModel
    {
        [Display(Name = "Giá trị Hợp đồng")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string Amount { get; set; }
    }
}