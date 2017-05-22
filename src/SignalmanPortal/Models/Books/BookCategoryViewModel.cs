using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.Books
{
    public class BookCategoryViewModel : BookCategory
    {
        public BookCategoryViewModel()
        {

        }
        public BookCategoryViewModel(BookCategory category)
        {
            CategoryId = category.CategoryId;
            Name = category.Name;
        }

        public bool IsRemoved { get; set; }
    }
}
