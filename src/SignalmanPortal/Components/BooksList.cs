using Microsoft.AspNetCore.Mvc;
using SignalmanPortal.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.Components
{
    public class BooksList : ViewComponent
    {
        private readonly IBooksRepository _booksRepository;
        public BooksList(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public IViewComponentResult Invoke(string categoryName)
        {
            var books = _booksRepository.Books;

            if(categoryName != null && categoryName != "Все")
            {
                books = books.Where(x => x.Category != null && x.Category.Name == categoryName);
            }

            return View(books);
        }
    }
}
