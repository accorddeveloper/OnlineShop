using System.Web.Mvc;

namespace OnlineShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductImages = new HashSet<ProductImage>();
        }

        public int Id { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public decimal Stock { get; set; }

        [StringLength(1000)]
        public string ImageUrl { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Notes { get; set; }

        public decimal Discount { get; set; }

        [StringLength(50)]
        public string DisplayPosition { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public int SortOrder { get; set; }

        

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
