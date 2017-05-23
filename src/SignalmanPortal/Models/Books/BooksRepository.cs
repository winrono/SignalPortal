using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        private IMapper _mapper;
        public BooksRepository(ApplicationDbContext dbContext, IHostingEnvironment hostingEnvironment, IMapper mapper)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        public IEnumerable<Book> Books
        {
            get
            {
                return _dbContext.Books.Include(x => x.Category);
            }
        }

        public IEnumerable<BookCategory> BookCategories
        {
            get
            {
                return _dbContext.CategoriesOfBooks.Where(x => x.DeletedOn == null);
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
                dbBook.CategoryId = book.CategoryId;
                //dbBook.ImageExtension = book.ImageExtension;
                dbBook.Name = book.Name;
            }

            _dbContext.SaveChanges();
        }

        public void InsertBook(Book book, IFormFile uploadedImage, IFormFile uploadedFile)
        {
            var imageExtension = Path.GetExtension(uploadedImage.FileName);
            var fileExtension = Path.GetExtension(uploadedFile.FileName);

            book.ImageExtension = imageExtension;
            book.FileExtension = fileExtension;

            _dbContext.Books.Add(book);

            _dbContext.SaveChanges();

            string bookPath = _hostingEnvironment.WebRootPath + "\\images\\books\\" + book.BookId + imageExtension;

            SaveFile(bookPath, uploadedImage);

            string filePath = _hostingEnvironment.WebRootPath + "\\data\\books\\" + book.BookId + fileExtension;

            SaveFile(filePath, uploadedFile);
        }

        public void CreateCategory(string name)
        {
            _dbContext.CategoriesOfBooks.Add(new BookCategory() { Name = name });

            _dbContext.SaveChanges();
        }

        public Book GetBookById(int id)
        {
            return _dbContext.Books.SingleOrDefault(x => x.BookId == id);
        }

        public bool DeleteBookCategory(int id)
        {
            var dbBookCategory = GetBookCategoryById(id);

            if (dbBookCategory != null)
            {
                _dbContext.Remove(dbBookCategory);

                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SaveCategories(IEnumerable<BookCategoryViewModel> categories)
        {
            foreach (var category in categories)
            {
                var dbCategory = _dbContext.CategoriesOfBooks.SingleOrDefault(x => x.CategoryId == category.CategoryId);

                if (dbCategory != null)
                {
                    if (category.IsRemoved == true)
                    {
                        dbCategory.DeletedOn = DateTime.Now;
                    }
                    else if (dbCategory.Name != category.Name)
                    {
                        dbCategory.Name = category.Name;
                    }
                }
                else if (category.IsRemoved != true)
                {
                    _dbContext.CategoriesOfBooks.Add(_mapper.Map<BookCategoryViewModel, BookCategory>(category));
                }
            }

            _dbContext.SaveChanges();

            return true;
        }

        private BookCategory GetBookCategoryById(int id)
        {
            return _dbContext.CategoriesOfBooks.SingleOrDefault(x => x.CategoryId == id);
        }

        private void SaveFile(string path, IFormFile file)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }


    }
}
