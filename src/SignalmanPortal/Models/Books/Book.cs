using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.Books
{
    public class Book
    {
        public int BookId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        public string ImageExtension { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        
        public virtual BookCategory Category { get; set; }
    }
}
