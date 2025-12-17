using System.ComponentModel.DataAnnotations;

namespace AdminService.Models
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }

        public string Questions { get; set; }

        public string Answers { get; set; }
    }
}
