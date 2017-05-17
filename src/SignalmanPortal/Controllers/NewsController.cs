using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SignalmanPortal.Models.News;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalmanPortal.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;
        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<Novelty> news = _newsRepository.News;
            return View(news);
        }

        public IActionResult Details(int id)
        {
            var model = _newsRepository.getNoveltyById(id);
            return View(model);
        }
    }
}
