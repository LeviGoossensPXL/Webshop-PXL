using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Entities;

namespace Webshop.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        // public DbSet<User> Users { get; set; }

        //// Tabel voor de algemene bestelinformatie
        public DbSet<Order> Orders { get; set; }

        //// Tabel voor de specifieke producten binnen een bestelling
        public DbSet<OrderLine> OrderLines { get; set; }

        //// TODO: dit is optioneel, afhankelijk van hoe We de adressen willen beheren. we kunnen ook overwegen om adressen direct in de Order-tabel op te slaan als JSON of als losse kolommen.
        public DbSet<Address> Adressen { get; set; }
    }

}
