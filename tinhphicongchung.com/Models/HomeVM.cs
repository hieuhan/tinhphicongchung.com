﻿using System.Collections.Generic;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Models
{
    public class HomeVM : BaseViewModel
    {
        
        public List<MenuItems> MenuItemsList { get; set; }
        public List<MenuItems> MobileMenuItemsList { get; set; }
    }
}