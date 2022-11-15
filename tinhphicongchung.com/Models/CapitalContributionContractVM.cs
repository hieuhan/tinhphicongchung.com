using System.ComponentModel.DataAnnotations;

namespace tinhphicongchung.com.Models
{
    public class CapitalContributionContractVM : BaseViewModel
    {
        [Display(Name = "Tổng giá trị vốn góp bằng tiền")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        public string Amount { get; set; }
    }
}