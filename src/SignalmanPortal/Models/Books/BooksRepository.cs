using SignalmanPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.Books
{
    public class BooksRepository : IBooksRepository
    {
        private ApplicationDbContext _dbContext;
        public BooksRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool DeleteBookById(int id)
        {
            var dbBook = GetBookById(id);

            if (dbBook != null)
            {
                _dbContext.Remove(dbBook);

                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EditBook(Book book)
        {
            var dbBook = GetBookById(book.BookId);
            if (dbBook != null)
            {
                dbBook.Description = book.Description;
                dbBook.ImagePath = book.ImagePath;
                dbBook.Name = book.Name;
                dbBook.Price = book.Price;
            }

            _dbContext.SaveChanges();
        }

        public void InsertBook(Book book)
        {
            _dbContext.Books.Add(book);
            _dbContext.SaveChanges();
        }

        private Book GetBookById(int id)
        {
            return _dbContext.Books.SingleOrDefault(x => x.BookId == id);
        }
    }
}
