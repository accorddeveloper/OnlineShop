using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using OnlineShop.Models;

namespace OnlineShop.Repositories
{
    public class CategoryRepository : BaseRepository
    {
        public static IList<Category> GetAll()
        {
            var result = db.Categories.OrderBy(x => x.SortOrder).ToList();
            return result;
        }

        public static IList<Category> GetAllByStatus(string status)
        {
            var result = db.Categories
                    .Where(x => x.Status == status)
                    .OrderBy(x => x.SortOrder).ToList();
            return result;
        }

        public static IList<Category> GetAllSortByName()
        {
            var result = db.Categories.OrderBy(x => x.Name).ToList();
            return result;
        }
    }
}