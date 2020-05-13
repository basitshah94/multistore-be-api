using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace multi_store.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Classification> Classifications { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<Image> Images { get; set; }
       
            // public DbSet<UserAuthentication> UserAuthentication { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
          modelBuilder.Entity<Role>().HasData(
           new Role
          {
              Id=1,
              Title = "Admin"
          },
          new Role
          {
              Id=2,
              Title = "Service_Provider"
          },
          new Role
          {
              Id=3,
              Title = "Service_Consumer"
          }
        );

         modelBuilder.Entity<Shop>()
                            .Property(r => r.IsVerified)
                            .HasConversion(new BoolToZeroOneConverter<Int16>());

         modelBuilder.Entity<Shop>()
                            .Property(r => r.IsDisabled)
                            .HasConversion(new BoolToZeroOneConverter<Int16>());

         modelBuilder.Entity<Product>()
                            .Property(r => r.IsDisabled)
                            .HasConversion(new BoolToZeroOneConverter<Int16>());

         modelBuilder.Entity<Product>()
                            .Property(r => r.IsAllowed)
                            .HasConversion(new BoolToZeroOneConverter<Int16>());
        }

    }
}