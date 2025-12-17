using CustomerService.DbContexts;
using CustomerService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Script.Services;
using System.Web.Services;

namespace CustomerService
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class CustomerService : WebService
    {
        private readonly AppDbContext dbContext = new AppDbContext();

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<bool> LogIn(string email, string password)
        {
            var customer = dbContext.Customer.FirstOrDefault(c => c.Email == email && c.Password == password);

            if (customer == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            return new ServiceResponse<bool>
            {
                Success = true,
                Message = "Login successful"
            };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Customer> GetCustomer(int id)
        {
            return GetById(dbContext.Customer, id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<List<Customer>> GetCustomers()
        {
            return GetAll(dbContext.Customer);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Customer> AddCustomer(Customer customer)
        {
            return AddEntity(customer, dbContext.Customer);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Customer> EditCustomer(Customer customer)
        {
            return EditEntity(customer, dbContext.Customer, c => c.Id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Customer> DeleteCustomer(int id)
        {
            return DeleteEntity(dbContext.Customer, c => c.Id == id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Order> GetOrder(int id)
        {
            return GetById(dbContext.Order, id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<List<Order>> GetOrders(int customerId)
        {
            return GetAll(dbContext.Order, o => o.CustomerId == customerId);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Order> AddOrder(Order order)
        {
            return AddEntity(order, dbContext.Order);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Order> DeleteOrder(int id)
        {
            return DeleteEntity(dbContext.Order, o => o.Id == id);
        }

        private ServiceResponse<T> GetById<T>(DbSet<T> dbSet, int id) where T : class
        {
            try
            {
                var entity = dbSet.Find(id);

                if (entity == null)
                {
                    return new ServiceResponse<T>
                    {
                        Success = false,
                        Message = $"{typeof(T).Name} not found"
                    };
                }

                return new ServiceResponse<T>
                {
                    Success = true,
                    Message = $"{typeof(T).Name} found",
                    Data = entity
                };
            }
            catch (Exception)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = "An unexpected error occurred"
                };
            }
        }

        private ServiceResponse<List<T>> GetAll<T>(DbSet<T> dbSet, Expression<Func<T, bool>> filter = null) where T : class
        {
            try
            {
                IQueryable<T> query = dbSet;

                if (filter != null) query = query.Where(filter);

                var entities = query.ToList();

                return new ServiceResponse<List<T>>
                {
                    Success = true,
                    Message = $"{typeof(T).Name}s loaded",
                    Data = entities
                };
            }
            catch (Exception)
            {
                return new ServiceResponse<List<T>>
                {
                    Success = false,
                    Message = $"Failed to load {typeof(T).Name}s"
                };
            }
        }

        private ServiceResponse<T> AddEntity<T>(T entity, DbSet<T> dbSet) where T : class
        {
            if (entity == null)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = $"Invalid {typeof(T).Name} data"
                };
            }

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                entity,
                new ValidationContext(entity),
                results,
                validateAllProperties: true
            );

            if (!isValid)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = "Validation Failed",
                    Errors = results.Select(r => r.ErrorMessage).ToList()
                };
            }

            try
            {
                dbSet.Add(entity);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = $"Failed to add {typeof(T).Name}"
                };
            }

            return new ServiceResponse<T>
            {
                Success = true,
                Message = $"{typeof(T).Name} added successfully",
            };
        }

        private ServiceResponse<T> EditEntity<T>(T entity, DbSet<T> dbSet, Func<T, object> keySelector) where T : class
        {
            if (entity == null)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = $"Invalid {typeof(T).Name} data"
                };
            }

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                entity,
                new ValidationContext(entity),
                results,
                true
            );

            if (!isValid)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = "Validation Failed",
                    Errors = results.Select(r => r.ErrorMessage).ToList()
                };
            }

            var key = keySelector(entity);

            var existingEntity = dbSet.Find(key);

            if (existingEntity == null)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = $"{typeof(T).Name} not found"
                };
            }

            try
            {
                dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = $"Failed to update {typeof(T).Name}"
                };
            }

            return new ServiceResponse<T>
            {
                Success = true,
                Message = $"{typeof(T).Name} updated successfully"
            };
        }

        private ServiceResponse<T> DeleteEntity<T>(DbSet<T> dbSet, Expression<Func<T, bool>> predicate) where T : class
        {
            var entity = dbSet.FirstOrDefault(predicate);

            if (entity == null)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = $"{typeof(T).Name} not found"
                };
            }

            try
            {
                dbSet.Remove(entity);
                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return new ServiceResponse<T>
                {
                    Success = false,
                    Message = $"Failed to delete {typeof(T).Name}"
                };
            }

            return new ServiceResponse<T>
            {
                Success = true,
                Message = $"{typeof(T).Name} deleted successfully"
            };
        }
    }
}
