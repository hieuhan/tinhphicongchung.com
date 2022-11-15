using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Models
{
    public class HeaderVM : BaseViewModel
    {
        public List<MenuItems> HeaderMenusList { get; set; }
        public List<MenuItems> HeaderMobileMenusList { get; set; }
    }
}