using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalmanPortal.Models.Books;
using Microsoft.AspNetCore.Hosting;

namespace SignalmanPortal.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IHostingEnvironment _environment;
        public BooksController(IBooksRepository booksRepository, IHostingEnvironment environment)
        {
            _booksRepository = booksRepository;
            _environment = environment;
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

        public FileResult DownloadBook(int id, string fileExtension)
        {
            var fileName = id + fileExtension;
            var filePath = _environment.WebRootPath + "\\data\\books\\" + id + fileExtension;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/x-msdownload", fileName);
        }
    }
}