using SignalmanPortal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.News
{
    public class NewsRepository : INewsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public NewsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Novelty> News
        {
            get
            {
                return _dbContext.News;
            }
        }

        public bool DeleteNoveltyById(int id)
        {
            var dbNovelty = getNoveltyById(id);
            if (dbNovelty != null)
            {
                _dbContext.News.Remove(dbNovelty);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EditNovelty(Novelty novelty)
        {
            var dbNovelty = getNoveltyById(novelty.NoveltyId);
            if (dbNovelty != null)
            {
                dbNovelty.Description = novelty.Description;
                dbNovelty.Title = novelty.Title;
            }

            _dbContext.SaveChanges();
        }

        public void InsertNovelty(Novelty novelty)
        {
            _dbContext.News.Add(novelty);

            _dbContext.SaveChanges();
        }

        private Novelty getNoveltyById(int id)
        {
            return _dbContext.News.SingleOrDefault(n => n.NoveltyId == id);
        }
    }
}
