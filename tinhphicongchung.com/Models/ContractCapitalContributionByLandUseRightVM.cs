using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Models
{
    public class ContractCapitalContributionByLandUseRightVM : BaseViewModel
    {
        [Display(Name = "Quận/Huyện")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int DistrictId { get; set; }

        [Display(Name = "Phường/Xã")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int WardId { get; set; }

        [Display(Name = "Đường/Phố")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int StreetId { get; set; }

        [Display(Name = "Loại đất")]
        [Range(1, byte.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public byte LandTypeId { get; set; }

        [Display(Name = "Vị trí")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int LocationId { get; set; }

        [Display(Name = "Diện tích Đất")]
        [Required(ErrorMessage = "Vui lòng nhập {0}")]
        [RegularExpression("^[0-9]+\\.?[0-9]+$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        [MaxLength(9, ErrorMessage = "Vui lòng nhập {0} hợp lệ, tối đa 9 ký tự")]
        public string LandArea { get; set; }

        public List<Districts> DistrictsList { get; set; }
        public List<Wards> WardsList { get; set; }
        public List<Streets> StreetsList { get; set; }
        public List<LandTypes> LandTypesList { get; set; }
        public List<LocationTypes> LocationTypesList { get; set; }
    }
}