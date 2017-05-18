using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.Books
{
    public class BookCreateViewModel
    {
        public Book Book { get; set; }
        public IEnumerable<BookCategory> Categories { get; set; }
    }
}
