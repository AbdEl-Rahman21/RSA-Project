using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminService.Models
{
    public class CartProduct
    {
        [Key, Column(Order = 0)]
        public int CartId { get; set; }

        [Key, Column(Order = 1)]
        public int ProductId { get; set; }

        public int ProductCount { get; set; }

        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}
