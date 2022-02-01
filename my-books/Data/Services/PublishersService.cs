using my_books.Data.Models;
using my_books.Data.Paging;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace my_books.Data.Services
{
    public class PublishersService
    {
        AppDbContext _context;
        public PublishersService(AppDbContext context)
        {
            _context = context;
        }

        public List<Publisher> GetAllPublishers(string sortBy, string searchString, int? pageNumber) 
        { 
            var allPublishers = _context.Publishers.OrderBy(n => n.Name).ToList(); //Default sort by ASC

            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        allPublishers = allPublishers.OrderByDescending(n => n.Name).ToList();
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                allPublishers = allPublishers.Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            //Paging
            int pageSize = 5;
            allPublishers = PaginatedList<Publisher>.Create(allPublishers.AsQueryable(), pageNumber ?? 1, pageSize);

            return allPublishers;
        }
        public Publisher AddPublisher(PublisherVM publisher)
        {
            if(StringStartsWithNumber(publisher.Name)) throw new PublisherNameException("Name starts with number", publisher.Name);                 

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
            else
            {
                throw new Exception($"Publisher with Id: {Id} doesn't exists");
            }
        }

        public bool StringStartsWithNumber(string name) => Regex.IsMatch(name, @"^\d");
    }
}
