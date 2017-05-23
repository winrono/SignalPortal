using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.Books
{
    public class BookCategory
    {
        [Key]
        public int CategoryId { get; set; }
        [Display(Name = "Название категории")]
        public string Name { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
