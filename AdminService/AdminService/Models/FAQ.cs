using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
