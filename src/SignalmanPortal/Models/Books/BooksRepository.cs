using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SignalmanPortal.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.Books
{
    public class BooksRepository : IBooksRepository
    {
        private ApplicationDbContext _dbContext;
        private IHostingEnvironment _hostingEnvironment;
        public BooksRepository(ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<Book> Books
        {
            get
            {
                return _dbContext.Books;
            }
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
                //dbBook.ImageExtension = book.ImageExtension;
                dbBook.Name = book.Name;
                dbBook.Price = book.Price;
            }

            _dbContext.SaveChanges();
        }

        public void InsertBook(Book book, IFormFile uploadedFile)
        {
            var fileExtension = Path.GetExtension(uploadedFile.FileName);

            book.ImageExtension = fileExtension;

            _dbContext.Books.Add(book);

            _dbContext.SaveChanges();

            string bookPath = _hostingEnvironment.WebRootPath + "\\images\\books\\" + book.BookId + "." + fileExtension;

            Directory.CreateDirectory(Path.GetDirectoryName(bookPath));

            using (var fileStream = new FileStream(bookPath, FileMode.Create))
            {
                uploadedFile.CopyTo(fileStream);
            }
        }

        public Book GetBookById(int id)
        {
            return _dbContext.Books.SingleOrDefault(x => x.BookId == id);
        }
    }
}
