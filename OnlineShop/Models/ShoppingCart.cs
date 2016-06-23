using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class ShoppingCartItem
    {
        public Product Item { set; get; }
        public int Quantity { set; get; }
    }

    public class ShoppingCart
    {
        public IList<ShoppingCartItem> ShoppingCartItems { set; get; }

        public ShoppingCart()
        {
            this.ShoppingCartItems = new List<ShoppingCartItem>();
        }

        public void AddToCart(Product product, int quantity)
        {
            var exists = this.ShoppingCartItems.FirstOrDefault(x => x.Item.Id == product.Id);
            if (exists != null)
            {
                exists.Quantity = exists.Quantity + quantity;
            }
            else
            {
                var item = new ShoppingCartItem();
                item.Item = product;
                item.Quantity = quantity;

                this.ShoppingCartItems.Add(item);
            }
        }

        public void UpdateCart(Product product, int quantity)
        {
            var exists = this.ShoppingCartItems.FirstOrDefault(x => x.Item.Id == product.Id);
            if (exists != null)
            {
                exists.Quantity = quantity;
            }
            else
            {
                var item = new ShoppingCartItem();
                item.Item = product;
                item.Quantity = quantity;

                this.ShoppingCartItems.Add(item);
            }
        }

        public void RemoveCart(int productId)
        {
            var exists = this.ShoppingCartItems.FirstOrDefault(x => x.Item.Id == productId);
            if (exists != null)
            {
                this.ShoppingCartItems.Remove(exists);
            }           
        }

    }
}