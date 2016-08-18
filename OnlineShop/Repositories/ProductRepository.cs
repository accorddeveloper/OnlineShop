using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnlineShop.Models;

namespace OnlineShop.Repositories
{
    public class ProductRepository : BaseRepository
    {
        public static IList<Product> GetAll()
        {
            var result = db.Products.OrderBy(x => x.SortOrder).ToList();
            return result;
        } 
    }
}