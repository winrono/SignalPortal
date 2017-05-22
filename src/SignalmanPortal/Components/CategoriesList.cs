using Microsoft.AspNetCore.Mvc;
using SignalmanPortal.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Components
{
    public class CategoriesList : ViewComponent
    {
        private readonly IBooksRepository _booksRepository;
        public CategoriesList(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public IViewComponentResult Invoke(string category)
        {
            if (!String.IsNullOrEmpty(category))
            {
                _booksRepository.CreateCategory(category);
            }

            List<BookCategoryViewModel> model = new List<BookCategoryViewModel>();
            foreach (var bookCategory in _booksRepository.BookCategories)
            {
                model.Add(new BookCategoryViewModel(bookCategory));
            }

            return View(model);
        }
    }
}
