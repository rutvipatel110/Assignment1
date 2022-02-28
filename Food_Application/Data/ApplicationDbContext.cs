using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Food_Application.Data;
using Food_Application.Models;

namespace Food_Application.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FoodItems> FoodItems { get; set; }
        public DbSet<SpecialTag> SpecialTag { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Order>    Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }



          
    }
}
