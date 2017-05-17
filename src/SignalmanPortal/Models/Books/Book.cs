using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "Стоимость")]
        public decimal Price { get; set; }
    }
}
