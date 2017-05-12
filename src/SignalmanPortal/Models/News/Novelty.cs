using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.News
{
    public class Novelty
    {
        public int NoveltyId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
