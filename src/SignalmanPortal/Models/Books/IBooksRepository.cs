using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal.Models.Books
{
    public interface IBooksRepository
    {
        void InsertBook(Book book);

        void EditBook(Book book);

        bool DeleteBookById(int id);

    }
}
