using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        public string GetCartNumbers()
        {
            if (Session["ShoppingCart"] != null)
            {
                var shoppingCart = (ShoppingCart)Session["ShoppingCart"];
                return shoppingCart.ShoppingCartItems.Count.ToString();
            }

            return "0";
        }

        public JsonResult GetShoppingCartItems()
        {
            // XE HÀNG
            var shoppingCart = new ShoppingCart();
            if (Session["ShoppingCart"] != null)
            {
                shoppingCart = (ShoppingCart)Session["ShoppingCart"];
            }
            return Json(shoppingCart.ShoppingCartItems.Select(x => new { x.Item.Id, x.Item.Name, x.Quantity, x.Item.Price, x.Item.Discount, x.Item.ImageUrl }).ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: ShoppingCart
        [HttpPost]
        public ActionResult AddToCart(int ProductId, int Quantity)
        {

            using (var db = new OnlineShopDb())
            {

                // TÌM PRODUCT THEO ProductId
                var product = db.Products.Find(ProductId);
                if (product != null)
                {
                    // XE HÀNG
                    var shoppingCart = new ShoppingCart();
                    if (Session["ShoppingCart"] != null)
                    {
                        shoppingCart = (ShoppingCart)Session["ShoppingCart"];
                    }

                    shoppingCart.AddToCart(product, Quantity);

                    Session["ShoppingCart"] = shoppingCart;

                    return RedirectToAction("CheckOut", "ShoppingCart");
                }

                return HttpNotFound();
            }
        }

        public void updateCart(int ProductId, int Quantity)
        {

            using (var db = new OnlineShopDb())
            {

                // TÌM PRODUCT THEO ProductId
                var product = db.Products.Find(ProductId);
                if (product != null)
                {
                    // XE HÀNG
                    var shoppingCart = new ShoppingCart();
                    if (Session["ShoppingCart"] != null)
                    {
                        shoppingCart = (ShoppingCart)Session["ShoppingCart"];
                    }

                    shoppingCart.UpdateCart(product, Quantity);

                    Session["ShoppingCart"] = shoppingCart;
                }
            }
        }

        [HttpPost]
        public ActionResult UpdateShoppingCart(FormCollection collection)
        {
            for (var i = 1; i <= collection.AllKeys.Length / 2; i++)
            {
                var productId = Convert.ToInt32(collection["ProductId_" + i]);
                var quantity = Convert.ToInt32(collection["Quantity_" + i]);

                updateCart(productId, quantity);
            }
            return View("CheckOut");
        }

        [HttpPost]
        public ActionResult RemoveCart(int ProductId)
        {
            using (var db = new OnlineShopDb())
            {
                // TÌM PRODUCT THEO ProductId
                var product = db.Products.Find(ProductId);
                if (product != null)
                {
                    // XE HÀNG
                    var shoppingCart = new ShoppingCart();
                    if (Session["ShoppingCart"] != null)
                    {
                        shoppingCart = (ShoppingCart)Session["ShoppingCart"];
                    }

                    shoppingCart.RemoveCart(product.Id);

                    Session["ShoppingCart"] = shoppingCart;
                }
            }

            return View("CheckOut");
        }

        public ActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut([Bind(Include = "ContactName,ContactAddress,ContactPhone,ContactEmail")] Order order)
        {
            using (var db = new OnlineShopDb())
            {

                order.CreatedDate = DateTime.Now;
                order.Status = "PENDING";
                order.CustomerId = null; // var userId = User.Identity.GetUserId();
                order.Code = Guid.NewGuid().ToString();

                var shoppingCart = (ShoppingCart)Session["ShoppingCart"];
                foreach (var item in shoppingCart.ShoppingCartItems)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductId = item.Item.Id;
                    orderDetail.Quantity = item.Quantity;
                    orderDetail.Discount = item.Item.Discount;
                    orderDetail.Price = item.Item.Price;

                    order.OrderDetails.Add(orderDetail);
                }

                db.Orders.Add(order);
                db.SaveChanges();

                Session["ShoppingCart"] = null;
                return RedirectToAction("Index", "Home");



            }
        }
    }
}