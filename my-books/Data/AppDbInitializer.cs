using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using my_books.Data.Models;
using System;
using System.Linq;

namespace my_books.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!context.Books.Any()) 
                { 
                    context.Books.AddRange(
                        new Book() 
                        { 
                            Title = "1st Book Title",
                            Description = "1st Book Description",
                            IsRead = true,
                            DateRead = DateTime.Now.AddDays(-10),
                            Rate = 4,
                            Gener = "Biography",
                            Author = "First Author",
                            DateAdded = DateTime.Now,
                            CoverUrl = "https..."
                        }, 
                        new Book() 
                        {
                            Title = "2nd Book Title",
                            Description = "2nd Book Description",
                            IsRead = false,
                            Gener = "Biography",
                            Author = "First Author",
                            DateAdded = DateTime.Now,
                            CoverUrl = "https..."
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
