using AdminService.DbContexts;
using AdminService.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;

namespace AdminService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class AdminServices : WebService
    {
        private readonly AppDbContext dbContext = new AppDbContext();

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<bool> LogIn(string email, string password)
        {
            bool result = email == "admin@email.com" && password == "admin123";

            return new ServiceResponse<bool> { Success = result, Data = result };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Product> GetProduct(int id)
        {
            var product = dbContext.Product.Find(id);

            if (product == null)
                return new ServiceResponse<Product> { Success = false, Message = "Product Not Found" };

            return new ServiceResponse<Product> { Success = true, Message = "Product Found", Data = product };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<List<Product>> GetProducts()
        {
            return new ServiceResponse<List<Product>> { Success = true, Data = dbContext.Product.ToList() };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Product> AddProduct(Product product)
        {
            if (product == null) return new ServiceResponse<Product> { Success = false, Message = "Invalid product data" };

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                product,
                new ValidationContext(product),
                results,
                validateAllProperties: true
            );

            if (!isValid)
            {
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Validation Failed",
                    Errors = results.Select(r => r.ErrorMessage).ToList()
                };
            }

            dbContext.Product.Add(product);
            dbContext.SaveChanges();

            return new ServiceResponse<Product> { Success = true, Message = "Product Added", Data = product };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Product> EditProduct(Product product)
        {
            if (product == null) return new ServiceResponse<Product> { Success = false, Message = "Invalid product data" };

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                product,
                new ValidationContext(product),
                results,
                validateAllProperties: true
            );

            if (!isValid)
            {
                return new ServiceResponse<Product>
                {
                    Success = false,
                    Message = "Validation Failed",
                    Errors = results.Select(r => r.ErrorMessage).ToList()
                };
            }

            var existingProduct = dbContext.Product.Find(product.Id);

            if (existingProduct == null) return new ServiceResponse<Product> { Success = false, Message = "Product not found" };

            existingProduct = product;

            dbContext.SaveChanges();

            return new ServiceResponse<Product> { Success = true, Message = "Product Updated", Data = existingProduct };
        }

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public ServiceResponse<bool> DeleteProduct(int id)
        //{
        //    var product = dbContext.Product.Find(id);

        //    if (product == null) return new ServiceResponse<bool> { Success = false, Message = "Product not found" };

        //    dbContext.Product.Remove(product);
        //    dbContext.SaveChanges();

        //    return new ServiceResponse<bool> { Success = true, Message = "Product deleted successfully" };
        //}
    }
}
