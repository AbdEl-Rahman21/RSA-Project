using AdminService.DbContexts;
using AdminService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Services;

namespace AdminService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class AdminServices : System.Web.Services.WebService
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
                return new ServiceResponse<Product> { Success = false, Message = "Product Not Found!" };

            return new ServiceResponse<Product> { Success = true, Message = "Product Found!", Data = product };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<ICollection<Product>> GetProducts()
        {
            return new ServiceResponse<ICollection<Product>> { Success = true, Data = dbContext.Product.ToList() };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Product> AddMovie(Product product)
        {
            var errors = new List<string>();
            modelV
            if (Product == null)
            {
                errors.Add("Movie Data Required");

                return new ServiceResponse<Movie> { Success = false, Message = "Validation Failed", Errors = errors.ToArray() };
            }

            if (string.IsNullOrWhiteSpace(movie.Title)) errors.Add("Title is required");

            if (movie.Title.Length > 100) errors.Add("Title Must be at Most 100 Characters");

            if (string.IsNullOrWhiteSpace(movie.Director)) errors.Add("Director is required");

            if (movie.Director.Length > 50) errors.Add("Director Must be at Most 50 Characters");

            if (movie.Rating < 1 || movie.Rating > 10) errors.Add("Rating Must be Between 1 and 10");

            if (errors.Count > 0)
                return new ServiceResponse<Movie> { Success = false, Message = "Validation Failed", Errors = errors.ToArray() };

            movie.Id = nextId++;

            movies.Add(movie);

            return new ServiceResponse<Movie> { Success = true, Message = "Movie Added", Data = movie };
        }
    }
}
