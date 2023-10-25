using DataAccess.Contexts;
using DataAccess.Entities;
using DataAccess.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text;

namespace BitirmeProjesi.Controllers
{
    public class DatabaseController : Controller
    {
        [Obsolete("Bu method artık kullanılmamaktadır!")]
        public IActionResult Seed()
        {
            using (BitirmeProjesiContext db = new BitirmeProjesiContext())
            {
                var productEntities = db.Products.ToList();
                db.RemoveRange(productEntities);

                var categoryEntities = db.Categories.ToList();
                db.RemoveRange(categoryEntities);

                var userDetailEntities = db.UserDetails.ToList();
                db.UserDetails.RemoveRange(userDetailEntities);

                var userEntities = db.Users.ToList();
                db.Users.RemoveRange(userEntities);

                var roleEntities = db.Roles.ToList();
                db.Roles.RemoveRange(roleEntities);

                if (categoryEntities.Count > 0)
                {
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Products', RESEED, 0)");
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Categories', RESEED, 0)");
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Users', RESEED, 0)");
                    db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Roles', RESEED, 0)");
                }

                db.Categories.Add(new Category()
                {
                    Name = "Electronic",
                    Description = "This category includes electronic products. These products include computers, smartphones, tablets, televisions, headphones, speakers, cameras, etc. This category covers a range of technological devices that can be used for work or entertainment purposes.",
                    Products = new List<Product>()
                    {
                        new Product()
                        {
                            Name = "Computer",
                            UnitPrice = 1000,
                            StockQuantity = 10,
                            Description = "This is a greate computer!!!"
                        },
                        new Product()
                        {
                            Name = "Mobile phone",
                            UnitPrice = 200,
                            StockQuantity = 50,
                            Description = "This is a greate mobile phone."
                        },
                        new Product()
                        {
                            Name = "Tablet",
                            UnitPrice = 600,
                            StockQuantity = 30,
                            Description = "This is a greate tablet!"
                        },
                         new Product()
                        {
                            Name = "Smartwatch",
                            UnitPrice = 500,
                            StockQuantity = 20,
                            Description = "This is a greate Smartwatch."
                        }
                    }
                });
                db.Categories.Add(new Category()
                {
                    Name = "Clothing",
                    Description = "This category includes clothing and accessories. Various products such as women's, men's and children's clothing, shoes, jewelry, sunglasses, hats, etc. can be found. This category is designed to meet the needs of customers interested in style and fashion.",
                    Products = new List<Product>()
                    {
                        new Product()
                        {
                            Name = "Nike Air Jordan",
                            UnitPrice = 2500,
                            StockQuantity = 30,
                            Description = "This is a greate shoe."
                        },
                        new Product()
                        {
                            Name = "Gucci Jacquard Jogging Pant",
                            UnitPrice = 4500,
                            StockQuantity = 5,
                            Description = "this is a greate pant!"
                        },
                        new Product()
                        {
                            Name = "Nike Tech Fleece",
                            UnitPrice = 2500,
                            StockQuantity = 15,
                            Description = "BUY IT!!"
                        }
                    }
                });

                db.SaveChanges();

                db.Roles.Add(new Role()
                {
                    Name = "Admin",
                    Users = new List<User>()
                    {
                        new User()
                        {
                            Firstname = "Melik",
                            Lastname = "Baykal",
                            Username = "melikbaykal",
                            Password = "melik",
                            IsActive = true,
                            UserDetail = new UserDetail()
                            {
                                Address = "Ankara",
                                Gender = Gender.Male,
                                Email = "realtopg@ecommerce.com",
                            }
                        }
                    }
                });
                db.Roles.Add(new Role()
                {
                    Name = "User",
                    Users = new List<User>()
                    {
                        new User()
                        {
                            Firstname = "Elon",
                            Lastname = "Musk",
                            Username = "elonmusk",
                            Password = "elon",
                            IsActive = true,
                            UserDetail = new UserDetail()
                            {
                                Address = "America",
                                Gender = Gender.Male,
                                Email = "elon@ecommerce.com",
                            }
                        }
                    }
                });

                db.SaveChanges();
            }

            return Content("<label style=\"color:red;\"><b>The initial data has been successfully generated.</b></label>", "text/html", Encoding.UTF8);
        }
    }
}