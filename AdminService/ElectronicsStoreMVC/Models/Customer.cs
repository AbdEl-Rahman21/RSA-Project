using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStoreMVC.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        [MaxLength(200)]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        public virtual List<Order> Orders { get; set; }

        public Customer(CustomerReference.Customer serviceCustomer)
        {
            Id = serviceCustomer.Id;
            Username = serviceCustomer.Username;
            Email = serviceCustomer.Email;
            Password = serviceCustomer.Password;
            PhoneNumber = serviceCustomer.PhoneNumber;
            Address = serviceCustomer.Address;
        }

        public Customer(CustomerReference.ServiceResponseOfCustomer serviceCustomer)
        {
            Id = serviceCustomer.Data.Id;
            Username = serviceCustomer.Data.Username;
            Email = serviceCustomer.Data.Email;
            Password = serviceCustomer.Data.Password;
            PhoneNumber = serviceCustomer.Data.PhoneNumber;
            Address = serviceCustomer.Data.Address; 
        }

        public Customer() { }
    }
}
