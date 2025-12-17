using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual ICollection<CartProduct> CartProducts { get; set; }
    }
}
