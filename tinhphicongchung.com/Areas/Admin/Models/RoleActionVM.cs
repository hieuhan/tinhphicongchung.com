using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Areas.Admin.Models
{
    public class RoleActionVM : BaseViewModel
    {
        public int RoleId { get; set; }
        public int ActionId { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public string ParentDescription { get; set; }
        public Roles Roles { get; set; }
        public List<Actions> ActionsList { get; set; }
        public List<Actions> ParentActionsList { get; set; }
        public List<ActionStatus> ActionStatusList { get; set; }
    }
}