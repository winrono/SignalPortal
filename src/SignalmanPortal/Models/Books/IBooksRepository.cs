using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.Books
{
    public interface IBooksRepository
    {
        IEnumerable<Book> Books { get; }
        IEnumerable<BookCategory> BookCategories { get; }
        void InsertBook(Book book, IFormFile uploadedFile);

        void EditBook(Book book);

        bool DeleteBookById(int id);
        void CreateCategory(string name);

        Book GetBookById(int id);
    }
}
