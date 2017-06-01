using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalmanPortal.Models.Books;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;

namespace SignalmanPortal.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IHostingEnvironment _environment;
        private readonly IMapper _mapper;
        public BooksController(IBooksRepository booksRepository, IHostingEnvironment environment, IMapper mapper)
        {
            _booksRepository = booksRepository;
            _environment = environment;
            _mapper = mapper;
        }

        public IActionResult GetBookById(int id)
        {
            return new JsonResult(_booksRepository.GetBookById(id));
        }

        public IActionResult GetBooksForCategory(int? categoryId, int pageId, int itemsPerPage)
        {
            var books = _booksRepository.GetBooksPaginated(categoryId, pageId, itemsPerPage);

            return new JsonResult(books);
        }

        public IActionResult GetBookCategories()
        {
            List<BookCategoryViewModel> categories = _mapper.Map<IEnumerable<BookCategory>, List<BookCategoryViewModel>>(_booksRepository.BookCategories);

            return new JsonResult(categories);
        }

        public IActionResult CheckFileExistance(int id, string fileExtension)
        {
            var fileName = id + fileExtension;
            var filePath = _environment.WebRootPath + "\\data\\books\\" + id + fileExtension;
            if (System.IO.File.Exists(filePath))
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult DownloadBook(int id, string fileExtension)
        {
            var fileName = id + fileExtension;
            var filePath = _environment.WebRootPath + "\\data\\books\\" + id + fileExtension;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/x-msdownload", fileName);
        }
    }
}