using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Models
{
    public class DocumentDivideInheritanceApartmentVM : BaseViewModel
    {
        [Display(Name = "Diện tích Nhà")]
        //[Required(ErrorMessage = "Vui lòng nhập {0}")]
        //[RegularExpression("^[0-9]+\\.?[0-9]+$", ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        //[Range(1, double.MaxValue, ErrorMessage = "Vui lòng nhập {0} hợp lệ")]
        public string HouseArea { get; set; }

        [Display(Name = "loại Nhà")]
        //[Range(1, byte.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public byte ApartmentTypeId { get; set; }

        [Display(Name = "Năm xây dựng")]
        //[Range(1960, 2500, ErrorMessage = "Vui lòng chọn {0}")]
        public int Year { get; set; }

        [Display(Name = "Cấp công trình")]
        //[Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn {0}")]
        public int ApartmentId { get; set; }
        public List<ApartmentTypes> ApartmentTypesList { get; set; }
        public List<YearBuilts> YearBuiltsList { get; set; }
        public List<Apartments> ApartmentsList { get; set; }
    }
}