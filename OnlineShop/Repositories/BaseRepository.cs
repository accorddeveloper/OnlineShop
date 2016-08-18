using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineShop.Models;

namespace OnlineShop.Repositories
{
    public class BaseRepository
    {
        public static OnlineShopDb db = new OnlineShopDb();
    }    
}