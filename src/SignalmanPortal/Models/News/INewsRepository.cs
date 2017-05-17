using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.News
{
    public interface INewsRepository
    {
        IEnumerable<Novelty> News { get; }

        void InsertNovelty(Novelty novelty);

        void EditNovelty(Novelty novelty);

        bool DeleteNoveltyById(int id);

        Novelty getNoveltyById(int id);
    }
}
