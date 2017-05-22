using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalmanPortal.Models.Books;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalmanPortal.Components
{
    public class CategoryEditor : ViewComponent
    {
        private readonly IBooksRepository _booksRepository;
        public CategoryEditor(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public IViewComponentResult Invoke(string category)
        {
            return View();
        }
    }
}
