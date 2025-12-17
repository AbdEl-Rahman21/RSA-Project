using ElectronicsStoreMVC.AdminReference;
using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStoreMVC.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public int ProductCount { get; set; }

        [Required]
        public decimal ProductPrice { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public Order(CustomerReference.Order serviceOrder)
        {
            Id = serviceOrder.Id;
            Date = serviceOrder.Date;   
            Status = serviceOrder.Status;
            ProductCount = serviceOrder.ProductCount;
            ProductPrice = serviceOrder.ProductPrice;
            CustomerId = serviceOrder.CustomerId;
            ProductId = serviceOrder.ProductId;
        }

        
        public Order()
        {

        }
    }
}
