using AutoMapper;
using SignalmanPortal.Models.Books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalmanPortal
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookCategory, BookCategoryViewModel>();
            CreateMap<BookCategoryViewModel, BookCategory>();
        }
    }
}
