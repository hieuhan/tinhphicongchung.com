using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class BaseViewModel
    {
        public byte StatusId { get; set; }
        public byte ReviewStatusId { get; set; }
        public string Keywords { get; set; }
        public string PreviousPage { get; set; }
        public int SearchByType { get; set; }
        public byte SearchByDateType { get; set; }
        public int OrderByClauseId { get; set; }
        public int SearchByClauseId { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public PaginationVM Pagination { get; set; }

        public List<ObjectJsonVM> WardPrefixsList = new List<ObjectJsonVM>
        {
            new ObjectJsonVM
            {
                Id = 1,
                Name = "Phường"
            },
            new ObjectJsonVM
            {
                Id = 2,
                Name = "Xã"
            },
            new ObjectJsonVM
            {
                Id = 3,
                Name = "Thị trấn"
            }
        };

        public List<ObjectJsonVM> StreetPrefixsList = new List<ObjectJsonVM>
        {
            new ObjectJsonVM
            {
                Id = 1,
                Name = "Đường"
            },
            new ObjectJsonVM
            {
                Id = 2,
                Name = "Phố"
            }
        };
    }
}