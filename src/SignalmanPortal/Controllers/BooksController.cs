using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalmanPortal.Models.Books;

namespace SignalmanPortal.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksRepository;
        public BooksController(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }
        public IActionResult Index()
        {
            return View(_booksRepository.BookCategories);
        }

        public IActionResult Details(int id)
        {
            return View(_booksRepository.GetBookById(id));
        }

        public IActionResult GetUpdatedViewComponent(string categoryName)
        {
            return ViewComponent("BooksList", new { categoryName = categoryName });
        }
    }
}