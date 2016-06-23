namespace OnlineShop.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(250)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(250)]
        public string ContactAddress { get; set; }

        [Required]
        [StringLength(250)]
        public string ContactPhone { get; set; }

        [StringLength(250)]
        public string ContactEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(250)]
        public string CustomerId { get; set; }

        [StringLength(250)]
        public string ShipperId { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
