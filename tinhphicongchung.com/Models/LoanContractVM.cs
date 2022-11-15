using System.ComponentModel.DataAnnotations;

namespace tinhphicongchung.com.Models
{
    public class LoanContractVM : BaseViewModel
    {
        [Display(Name = "Tổng số tiền vay")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string Amount { get; set; }
    }
}