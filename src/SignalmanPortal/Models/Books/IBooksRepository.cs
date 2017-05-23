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
        IEnumerable<BookCategory> BookCategories { get;}
        void InsertBook(Book book, IFormFile uploadedImage, IFormFile uploadedFile);

        void EditBook(Book book);

        bool DeleteBookById(int id);
        void CreateCategory(string name);

        bool DeleteBookCategory(int id);

        bool SaveCategories(IEnumerable<BookCategoryViewModel> categories);

        Book GetBookById(int id);
    }
}
