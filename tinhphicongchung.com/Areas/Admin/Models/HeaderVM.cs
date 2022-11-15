using System.Collections.Generic;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class HeaderVM
    {
        public List<Actions> ActionsList { get; set; }
        public List<Actions> ParentActionsList { get; set; }
    }
}