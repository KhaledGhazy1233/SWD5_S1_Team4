using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;

namespace DataLayer.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductImage> ProductImages { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductOrder> ProductOrders { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<OrderHeader> OrderHeaders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>(entity => 
        {
            entity.Property(u => u.FirstName).IsRequired();
            entity.Property(u => u.LastName).IsRequired();
            entity.Property(u => u.Address).IsRequired();
            entity.Property(u => u.City).IsRequired();
            entity.Property(u => u.State).IsRequired();
            entity.Property(u => u.PostalCode).IsRequired();
        });
    
        // Coupon entity
        modelBuilder.Entity<Coupon>()
            .Property(c => c.Discount)
            .HasColumnType("decimal(18,2)");
    
        modelBuilder.Entity<Coupon>()
            .Property(c => c.MinimumAmount)
            .HasColumnType("decimal(18,2)");
    
        // Order entity
        modelBuilder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasColumnType("decimal(18,2)");
    
        modelBuilder.Entity<Order>()
            .Property(o => o.Discount)
            .HasColumnType("decimal(18,2)");
    
        modelBuilder.Entity<Order>()
            .Property(o => o.Fax)
            .HasColumnType("decimal(18,2)");
    
        modelBuilder.Entity<Order>()
            .Property(o => o.FinalPrice)
            .HasColumnType("decimal(18,2)");
    
        // Product entity
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");
    
        // ProductOrder entity
        modelBuilder.Entity<ProductOrder>()
            .Property(po => po.Price)
            .HasColumnType("decimal(18,2)");
    
        // ShoppingCart entity
        modelBuilder.Entity<ShoppingCart>()
            .Property(sc => sc.Price)
            .HasColumnType("decimal(18,2)");
    
        // Relationships
        modelBuilder.Entity<ProductOrder>()
            .HasKey(po => new { po.ProductId, po.OrderId });
        
        modelBuilder.Entity<ProductOrder>()
            .HasOne(po => po.Product)
            .WithMany(p => p.ProductOrders)
            .HasForeignKey(po => po.ProductId);
        
        modelBuilder.Entity<ProductOrder>()
            .HasOne(po => po.Order)
            .WithMany(o => o.ProductOrders)
            .HasForeignKey(po => po.OrderId);
    }
}