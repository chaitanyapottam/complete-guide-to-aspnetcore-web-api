using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace my_books.Data.Services
{
    public class PublishersService
    {
        AppDbContext _context;
        public PublishersService(AppDbContext context)
        {
            _context = context;
        }

        public Publisher AddPublisher(PublisherVM publisher)
        {
            var _publisher = new Publisher()
            {
                Name = publisher.Name
            };

            _context.Publishers.Add(_publisher);
            _context.SaveChanges();

            return _publisher;
        }

        public Publisher GetPublisherById(int Id)
        {
            return _context.Publishers.FirstOrDefault(n => n.Id == Id);
        }

        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId) 
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId).Select(n => new PublisherWithBooksAndAuthorsVM() 
            { 
                Name = n.Name,
                BookAuthors = n.Books.Select(n => new BookAuthorVM()
                {
                    BookTitle = n.Title,
                    BookAuthors = n.Book_Authors.Select(n => n.Author.FullName).ToList()
                }).ToList()
            }).FirstOrDefault();

            return _publisherData;
        }

        public void DeletePublisherById(int Id)
        {
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == Id);

            if(_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
        }
    }
}
