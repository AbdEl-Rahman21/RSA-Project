using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStoreMVC.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 99999999.99)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required]
        public int CountAvailable { get; set; }

        [Required]
        [MaxLength(100)]
        public string Category { get; set; }

        public virtual List<Order> Orders { get; set; }

        public Product(AdminReference.ServiceResponseOfProduct product)
        {
            Id = product.Data.Id;
            Name = product.Data.Name;
            Price = product.Data.Price;
            Description = product.Data.Description;
            CountAvailable = product.Data.CountAvailable;
            Category = product.Data.Category;
        }

        public Product(AdminReference.Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Description = product.Description;
            CountAvailable = product.CountAvailable;
            Category = product.Category;
        }

        public Product(CustomerReference.Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Price = product.Price;
            Description = product.Description;
            CountAvailable = product.CountAvailable;
            Category = product.Category;
        }
        public Product()
        {
            
        }
    }
}
