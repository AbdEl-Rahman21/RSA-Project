using CustomerService;
using CustomerService.DbContexts;
using CustomerService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace CustomerService
{
    /// <summary>
    /// Summary description for CustomerService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CustomerService : System.Web.Services.WebService
    {

        private readonly AppDbContext dbContext = new AppDbContext();

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<bool> LogIn(string email, string password)
        {
            bool result = false;
            var users = dbContext.Customer.ToList();
            foreach (Customer user in users)
            {
                if (user.Email == email)
                {
                    result = true;
                    break;
                }
                else { result = false; }
            }
             return new ServiceResponse<bool> { Success = result, Data = result };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Order> GetOrder(int id)
        {
            var product = dbContext.Order.Find(id);

            if (product == null)
                return new ServiceResponse<Order> { Success = false, Message = "Order Not Found" };

            return new ServiceResponse<Order> { Success = true, Message = "Order Found", Data = product };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<List<Order>> GetOrders()
        {
            return new ServiceResponse<List<Order>> { Success = true, Data = dbContext.Order.ToList() };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Order> AddOrder(Order order)
        {
            if (order == null) return new ServiceResponse<Order> { Success = false, Message = "Invalid order data" };

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                order,
                new ValidationContext(order),
                results,
                validateAllProperties: true
            );

            if (!isValid)
            {
                return new ServiceResponse<Order>
                {
                    Success = false,
                    Message = "Validation Failed",
                    Errors = results.Select(r => r.ErrorMessage).ToList()
                };
            }

            dbContext.Order.Add(order);
            dbContext.SaveChanges();

            return new ServiceResponse<Order> { Success = true, Message = "Product Added", Data = order };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Order> EditOrder(Order order)
        {
            if (order == null) return new ServiceResponse<Order> { Success = false, Message = "Invalid order data" };

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                order,
                new ValidationContext(order),
                results,
                validateAllProperties: true
            );

            if (!isValid)
            {
                return new ServiceResponse<Order>
                {
                    Success = false,
                    Message = "Validation Failed",
                    Errors = results.Select(r => r.ErrorMessage).ToList()
                };
            }

            var existingProduct = dbContext.Order.Find(order.Id);

            if (existingProduct == null) return new ServiceResponse<Order> { Success = false, Message = "Product not found" };

            existingProduct = order;

            dbContext.SaveChanges();

            return new ServiceResponse<Order> { Success = true, Message = "Product Updated", Data = existingProduct };
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public ServiceResponse<bool> DeleteProduct(int id)
        //{
        //    var order = dbContext.Product.Find(id);

        //    if (order == null) return new ServiceResponse<bool> { Success = false, Message = "Product not found" };

        //    dbContext.Product.Remove(order);
        //    dbContext.SaveChanges();

        //    return new ServiceResponse<bool> { Success = true, Message = "Product deleted successfully" };
        //}
    }
}
