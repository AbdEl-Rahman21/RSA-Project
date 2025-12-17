using AdminService.DbContexts;
using AdminService.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
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
            if (email == "admin@email.com" && password == "admin123")
            {
                return new ServiceResponse<bool>
                {
                    Success = true,
                    Message = "Login successful"
                };
            }

            return new ServiceResponse<bool>
            {
                Success = false,
                Message = "Invalid email or password"
            };
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Product> GetProduct(int id)
        {
            return GetById(dbContext.Product, id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<List<Product>> GetProducts()
        {
            return GetAll(dbContext.Product);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Product> AddProduct(Product product)
        {
            return AddEntity(product, dbContext.Product);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Product> EditProduct(Product product)
        {
            return EditEntity(
                product,
                dbContext.Product,
                p => p.Id,
                (existing, updated) =>
                {
                    if (existing.Price != updated.Price)
                    {
                        string message = $"Price for product '{updated.Name}' changed from {existing.Price:C} to {updated.Price:C}.";

                        Task.Run(() =>
                        {
                            try
                            {
                                var customers = dbContext.Customer.ToList();

                                foreach (var customer in customers)
                                {
                                    SendNotification(customer.Id, customer.Email, message, "Price Change");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Notification error: {ex.Message}");
                            }
                        });
                    }
                }
            );
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Product> DeleteProduct(int id)
        {
            return DeleteEntity(dbContext.Product, p => p.Id == id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<FAQ> GetFAQ(int id)
        {
            return GetById(dbContext.FAQ, id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<List<FAQ>> GetFAQs()
        {
            return GetAll(dbContext.FAQ);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<FAQ> AddFAQ(FAQ faq)
        {
            return AddEntity(faq, dbContext.FAQ);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<FAQ> EditFAQ(FAQ faq)
        {
            return EditEntity(faq, dbContext.FAQ, f => f.Id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<FAQ> DeleteFAQ(int id)
        {
            return DeleteEntity(dbContext.FAQ, f => f.Id == id);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public ServiceResponse<Order> ChangeOrderStatus(int orderId, string newStatus)
        {
            var order = dbContext.Order.Find(orderId);

            if (order == null)
            {
                return new ServiceResponse<Order>
                {
                    Success = false,
                    Message = "Order not found"
                };
            }

            if (!new[] { "Processing", "Shipped", "Delivered", "Cancelled" }.Contains(newStatus))
            {
                return new ServiceResponse<Order>
                {
                    Success = false,
                    Message = "Invaild status"
                };
            }

            string message = $"Order '{order.Id}' changed from {order.Status} to {newStatus}.";

            try
            {
                order.Status = newStatus;

                dbContext.SaveChanges();
            }
            catch (Exception)
            {
                return new ServiceResponse<Order>
                {
                    Success = false,
                    Message = "Failed to update Order"
                };
            }

            SendNotification(order.CustomerId, order.Customer.Email, message, "Order Status Update");

            return new ServiceResponse<Order>
            {
                Success = true,
                Message = "Order updated successfully"
            };
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

        private ServiceResponse<List<T>> GetAll<T>(DbSet<T> dbSet) where T : class
        {
            try
            {
                var entities = dbSet.ToList();

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

        private ServiceResponse<T> EditEntity<T>(T entity, DbSet<T> dbSet, Func<T, object> keySelector, Action<T, T> postUpdateAction = null) where T : class
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
                postUpdateAction?.Invoke(existingEntity, entity);

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

        private void SendNotification(int customerId, string email, string message, string subject)
        {
            var notification = new Notification
            {
                Message = message,
                CustomerId = customerId,
            };

            AddEntity(notification, dbContext.Notification);

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.office365.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(
                        "2022170928@cis.asu.edu.eg",
                        "Coj40431"
                    );

                    var mail = new MailMessage
                    {
                        From = new MailAddress("2022170928@cis.asu.edu.eg", "Electronic Store")
                    };

                    mail.To.Add(email);
                    mail.Subject = subject;
                    mail.Body = message;

                    smtp.Send(mail);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email to {email}: {ex.Message}");
            }
        }
    }
}
