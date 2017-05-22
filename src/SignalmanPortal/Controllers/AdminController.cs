using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalmanPortal.Models.News;
using SignalmanPortal.Data;
using Microsoft.AspNetCore.Authorization;
using SignalmanPortal.Models.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Collections;

namespace SignalmanPortal.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly IBooksRepository _booksRepository;
        private readonly ApplicationDbContext _dbContext;
        private IHostingEnvironment _hostingEnvironment;
        public AdminController(INewsRepository newsRepository, IBooksRepository booksRepository, ApplicationDbContext dbContext)
        {
            _newsRepository = newsRepository;
            _booksRepository = booksRepository;
            _dbContext = dbContext;
        }

        public IActionResult News()
        {
            var news = _newsRepository.News;
            return View(news);
        }

        [HttpGet]
        public IActionResult NoveltyEdit(int id)
        {
            var novelty = _newsRepository.News.SingleOrDefault(n => n.NoveltyId == id);

            if (novelty != null)
            {
                return View(novelty);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult NoveltyEdit(Novelty model)
        {
            _newsRepository.EditNovelty(model);

            return RedirectToAction("News");
        }

        [HttpGet]
        public IActionResult NoveltyCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NoveltyCreate(Novelty model)
        {
            model.DateAdded = DateTime.Now;

            _newsRepository.InsertNovelty(model);

            return RedirectToAction("News");
        }

        public bool NoveltyDelete(int id)
        {
            return _newsRepository.DeleteNoveltyById(id);
        }

        public IActionResult Books()
        {
            return View(_booksRepository.Books);
        }

        [HttpGet]
        public IActionResult BookEdit(int id)
        {
            var book = _booksRepository.Books.SingleOrDefault(n => n.BookId == id);

            if (book != null)
            {
                return View(book);
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost]
        public IActionResult BookEdit(Book model)
        {
            _booksRepository.EditBook(model);

            return RedirectToAction("Books");
        }

        [HttpGet]
        public IActionResult BookCreate()
        {
            var viewModel = new BookCreateViewModel();
            viewModel.Categories = _booksRepository.BookCategories;

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult BookCreate(BookCreateViewModel viewModel, IFormFile uploadedFile)
        {
            _booksRepository.InsertBook(viewModel.Book, uploadedFile);

            return RedirectToAction("Books");
        }

        [HttpGet]
        public IActionResult BookCategories()
        {

            return View();
        }

        public bool DeleteBookCategory(int id)
        {
            return _booksRepository.DeleteBookCategory(id);
        }

        [HttpPost]
        public IActionResult CreateBookCategory([FromBody] string name)
        {
            _booksRepository.CreateCategory(name);

            _dbContext.SaveChanges();

            return View("BookCategories");
        }

        [HttpPost]
        public IActionResult SaveCategories([FromBody] IEnumerable<BookCategoryViewModel> categories)
        {
            categories = categories.Where(x => x.IsRemoved == false);
            _booksRepository.SaveCategories(categories as IEnumerable<BookCategory>);

            _dbContext.SaveChanges();

            return RedirectToAction("BookCategories");
        }

        public IActionResult GetUpdatedCategoriesList(string category)
        {
            return ViewComponent("CategoriesList", new { category = category });
        }

        public bool BookDelete(int id)
        {
            return _booksRepository.DeleteBookById(id);
        }
    }
}