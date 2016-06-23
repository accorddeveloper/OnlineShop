namespace OnlineShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ProductImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(1000)]
        public string ImageUrl { get; set; }

        public virtual Product Product { get; set; }
    }
}
