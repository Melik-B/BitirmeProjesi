using AppCore.DataAccess.Configs;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class BitirmeProjesiContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<ProductStore> ProductStores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"server=MELIK\SQLEXPRESS;database=BitirmeProjesiV3;trusted_connection=true;multipleactiveresultsets=true;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(product => product.Category)
                .WithMany(category => category.Products)
                .HasForeignKey(product => product.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasOne(user => user.Role)
                .WithMany(role => role.Users)
            .HasForeignKey(user => user.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserDetail>()
            .HasOne(userDetail => userDetail.User)
                .WithOne(user => user.UserDetail)
                .HasForeignKey<UserDetail>(userDetail => userDetail.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductStore>()
                .HasKey(productStore => new { productStore.ProductId, productStore.StoreId });

            modelBuilder.Entity<Product>()
                .HasMany(product => product.ProductStores)
                .WithOne(productStore => productStore.Product)
                .HasForeignKey(productStore => productStore.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Store>()
                .HasMany(store => store.ProductStores)
                .WithOne(productStore => productStore.Store)
                .HasForeignKey(productStore => productStore.StoreId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

}
