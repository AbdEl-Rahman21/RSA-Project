using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStoreMVC.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public Notification(CustomerReference.Notification notification)
        {
            Id = notification.Id;
            Message = notification.Message;
            CreatedAt = notification.CreatedAt;
            CustomerId = notification.CustomerId;
            
        }
    }
}
