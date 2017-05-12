using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalmanPortal.Models.News;
using SignalmanPortal.Data;
using Microsoft.AspNetCore.Authorization;

namespace SignalmanPortal.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly ApplicationDbContext _dbContext;
        public AdminController(INewsRepository newsRepository, ApplicationDbContext dbContext)
        {
            _newsRepository = newsRepository;
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
            ModelState.Remove("NoveltyId");

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

        public IActionResult Learn()
        {
            return View();
        }
    }
}