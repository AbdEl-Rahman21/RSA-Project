using ElectronicsStoreMVC.AdminReference;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStoreMVC.Models
{
    public class FAQ
    {
        public FAQ(AdminReference.FAQ serviceFaq)
        {
            Id = serviceFaq.Id;
            Question = serviceFaq.Question;
            Answer = serviceFaq.Answer;
        }

        public FAQ(ServiceResponseOfFAQ serviceFaq)
        {
            Id = serviceFaq.Data.Id;
            Question = serviceFaq.Data.Question;
            Answer = serviceFaq.Data.Answer;
        }

        public FAQ() { }

        [Key]
        public int Id { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
