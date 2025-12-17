using System.ComponentModel.DataAnnotations;

namespace CustomerService.Models
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
