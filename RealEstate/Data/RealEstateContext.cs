using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;

namespace RealEstate.Data
{
    public class RealEstateContext : DbContext
    {
        public RealEstateContext(DbContextOptions<RealEstateContext> options)
            : base(options)
        {
        }

        public DbSet<RealEstate.Models.Account> Account { get; set; } = default!;
        public DbSet<RealEstate.Models.User> User { get; set; } = default!;
        public DbSet<RealEstate.Models.Category> Category { get; set; } = default!;
        public DbSet<RealEstate.Models.Product> Product { get; set; } = default!;
        public DbSet<RealEstate.Models.ProductImage> ProductImage { get; set; } = default!;
        public DbSet<RealEstate.Models.Transaction> Transaction { get; set; } = default!;
        public DbSet<RealEstate.Models.TransactionExcept> TransactionExcept { get; set; } = default!;
    }
}
