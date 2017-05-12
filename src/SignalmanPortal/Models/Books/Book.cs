using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.Books
{
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
