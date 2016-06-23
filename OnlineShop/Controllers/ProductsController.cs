using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class ProductsController : Controller
    {       
        public ActionResult Index(string sort)
        {
            using (var db = new OnlineShopDb())
            {
                var products = db.Products.OrderBy(x => x.Price); ;

                if (sort == "desc")
                {
                    products = products.OrderByDescending(x => x.Price);
                }

                return View(products.ToList());
            }
        }

        public ActionResult DisplayByCategory(int categoryId)
        {
            using (var db = new OnlineShopDb())
            {
                var products = db.Products.Where(x => x.CategoryId == categoryId).ToList();
                return View("Index", products);
            }
        }

        public ActionResult Details(int? id)
        {
            using (var db = new OnlineShopDb())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Product product = db.Products.Include(x => x.ProductImages).FirstOrDefault(x => x.Id == id);
                if (product == null)
                {
                    return HttpNotFound();
                }

                return View(product);
            }
        }

        [HttpPost]
        public ActionResult Search(string SearchText, string PriceMin, string PriceMax)
        {
            using (var db = new OnlineShopDb())
            {
                var min = 0;
                var max = Int32.MaxValue;

                if (string.IsNullOrEmpty(PriceMin) == false)
                {
                    min = Convert.ToInt32(PriceMin);
                }

                if (string.IsNullOrEmpty(PriceMax) == false)
                {
                    max = Convert.ToInt32(PriceMax);
                }

                var products = db.Products.Where(x => x.Name.Contains(SearchText) && (x.Price >= min && x.Price <= max)).ToList();


                return View("Index", products);
            }            
        }
    }
}