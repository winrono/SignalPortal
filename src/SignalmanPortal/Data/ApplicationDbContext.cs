﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalmanPortal.Models;
using SignalmanPortal.Models.News;
using SignalmanPortal.Models.Books;
using System.Linq;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SignalmanPortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }

        public DbSet<Novelty> News { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookCategory> CategoriesOfBooks { get; set; }

    }
}
