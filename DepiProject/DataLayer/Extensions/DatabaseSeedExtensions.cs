using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DataLayer.Extensions
{
    public static class DatabaseSeedExtensions
    {
        public static void SeedDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Run migrations if they haven't been applied
            dbContext.Database.Migrate();

            // Seed Categories
            SeedCategories(dbContext);

            // Seed Products
            SeedProducts(dbContext);

            // Seed Product Images
            SeedProductImages(dbContext);
        }

        private static void SeedCategories(ApplicationDbContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                var categories = new Category[]
                {
                    new Category
                    {
                        Name = "Laptops",
                        Description = "Portable computers for work and personal use",
                        ImageUrl = "/images/categories/laptops.png",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 498),
                        IsDeleted = false
                    },
                    new Category
                    {
                        Name = "Smartphones",
                        Description = "Mobile phones with advanced features",
                        ImageUrl = "/images/categories/smartphones.png",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 498),
                        IsDeleted = false
                    },
                    new Category
                    {
                        Name = "Accessories",
                        Description = "Various accessories for electronic devices",
                        ImageUrl = "/images/categories/accessories.png",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 498),
                        IsDeleted = false
                    },
                    new Category
                    {
                        Name = "Audio",
                        Description = "Sound equipment including headphones and speakers",
                        ImageUrl = "/images/categories/audio.png",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 498),
                        IsDeleted = false
                    },
                    new Category
                    {
                        Name = "Gaming",
                        Description = "Gaming consoles, games, and related accessories",
                        ImageUrl = "/images/categories/gaming.png",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 498),
                        IsDeleted = false
                    }
                };

                dbContext.Categories.AddRange(categories);
                dbContext.SaveChanges();
            }
        }

        private static void SeedProducts(ApplicationDbContext dbContext)
        {
            if (!dbContext.Products.Any())
            {
                // Get categories from DB to ensure correct IDs
                var categories = dbContext.Categories.ToDictionary(c => c.Name, c => c.CategoryId);

                var products = new Product[]
                {
                    new Product
                    {
                        // Remove explicit ProductId
                        Name = "MacBook Pro 16",
                        Description = "16-inch MacBook Pro with M2 Pro chip",
                        IsAvailable = true,
                        Price = 2499.99M,
                        StockQuantity = 25,
                        Brand = "Apple",
                        Model = "MacBook Pro 16",
                        TechnicalSpecifications = "M2 Pro chip, 16GB RAM, 512GB SSD, 16-inch Retina display",
                        DiscountPercentage = 5.00M,
                        IsFeatured = true,
                        WarrantyInfo = "1 year limited warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Laptops"]
                    },
                    new Product
                    {
                        // Remove explicit ProductId
                        Name = "Dell XPS 15",
                        Description = "15-inch premium Windows laptop",
                        IsAvailable = true,
                        Price = 1899.99M,
                        StockQuantity = 20,
                        Brand = "Dell",
                        Model = "XPS 15",
                        TechnicalSpecifications = "Intel Core i7, 16GB RAM, 1TB SSD, NVIDIA RTX 3050 Ti",
                        DiscountPercentage = 10.00M,
                        IsFeatured = true,
                        WarrantyInfo = "2 year warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Laptops"]
                    },
                    new Product
                    {
                        // Remove explicit ProductId
                        Name = "iPhone 15 Pro",
                        Description = "Apple's flagship smartphone with advanced features",
                        IsAvailable = true,
                        Price = 999.99M,
                        StockQuantity = 50,
                        Brand = "Apple",
                        Model = "iPhone 15 Pro",
                        TechnicalSpecifications = "A17 chip, 6.1-inch Super Retina XDR display, Triple camera system",
                        DiscountPercentage = 0.00M,
                        IsFeatured = true,
                        WarrantyInfo = "1 year limited warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Smartphones"]
                    },
                    new Product
                    {
                        Name = "Samsung Galaxy S23 Ultra",
                        Description = "Premium Android smartphone with S Pen",
                        IsAvailable = true,
                        Price = 1199.99M,
                        StockQuantity = 40,
                        Brand = "Samsung",
                        Model = "Galaxy S23 Ultra",
                        TechnicalSpecifications = "Snapdragon 8 Gen 2, 6.8-inch Dynamic AMOLED, 200MP camera",
                        DiscountPercentage = 15.00M,
                        IsFeatured = true,
                        WarrantyInfo = "1 year warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Smartphones"]
                    },
                    new Product
                    {
                        Name = "Magic Keyboard",
                        Description = "Wireless keyboard for Mac and iPad",
                        IsAvailable = true,
                        Price = 99.99M,
                        StockQuantity = 100,
                        Brand = "Apple",
                        Model = "Magic Keyboard",
                        TechnicalSpecifications = "Bluetooth, Rechargeable battery, Scissor mechanism",
                        DiscountPercentage = 0.00M,
                        IsFeatured = false,
                        WarrantyInfo = "1 year limited warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Accessories"]
                    },
                    new Product
                    {
                        // Remove explicit ProductId
                        Name = "Logitech MX Master 3",
                        Description = "Advanced wireless mouse for productivity",
                        IsAvailable = true,
                        Price = 99.99M,
                        StockQuantity = 75,
                        Brand = "Logitech",
                        Model = "MX Master 3",
                        TechnicalSpecifications = "Bluetooth or USB receiver, Rechargeable, 4000 DPI sensor",
                        DiscountPercentage = 10.00M,
                        IsFeatured = true,
                        WarrantyInfo = "2 year warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Accessories"]
                    },
                    new Product
                    {
                        // Remove explicit ProductId
                        Name = "AirPods Pro",
                        Description = "Wireless earbuds with active noise cancellation",
                        IsAvailable = true,
                        Price = 249.99M,
                        StockQuantity = 60,
                        Brand = "Apple",
                        Model = "AirPods Pro",
                        TechnicalSpecifications = "Active Noise Cancellation, Transparency mode, Spatial Audio",
                        DiscountPercentage = 5.00M,
                        IsFeatured = true,
                        WarrantyInfo = "1 year limited warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Audio"]
                    },
                    new Product
                    {
                        // Remove explicit ProductId
                        Name = "Sony WH-1000XM5",
                        Description = "Premium noise-cancelling headphones",
                        IsAvailable = true,
                        Price = 399.99M,
                        StockQuantity = 30,
                        Brand = "Sony",
                        Model = "WH-1000XM5",
                        TechnicalSpecifications = "Industry-leading noise cancellation, 30-hour battery life, LDAC codec",
                        DiscountPercentage = 10.00M,
                        IsFeatured = true,
                        WarrantyInfo = "2 year warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Audio"]
                    },
                    new Product
                    {
                        // Remove explicit ProductId
                        Name = "PlayStation 5",
                        Description = "Sony's next-generation gaming console",
                        IsAvailable = true,
                        Price = 499.99M,
                        StockQuantity = 15,
                        Brand = "Sony",
                        Model = "PlayStation 5",
                        TechnicalSpecifications = "AMD Zen 2 CPU, AMD RDNA 2 GPU, 825GB SSD, 4K gaming",
                        DiscountPercentage = 0.00M,
                        IsFeatured = true,
                        WarrantyInfo = "1 year limited warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Gaming"]
                    },
                    new Product
                    {
                        // Remove explicit ProductId
                        Name = "Xbox Series X",
                        Description = "Microsoft's flagship gaming console",
                        IsAvailable = true,
                        Price = 499.99M,
                        StockQuantity = 20,
                        Brand = "Microsoft",
                        Model = "Xbox Series X",
                        TechnicalSpecifications = "8-core AMD Zen 2 CPU, 12 teraflops GPU, 1TB SSD, 4K gaming",
                        DiscountPercentage = 5.00M,
                        IsFeatured = true,
                        WarrantyInfo = "1 year limited warranty",
                        CreatedAt = new DateTime(2025, 05, 15, 16, 58, 36, 774),
                        IsDeleted = false,
                        CategoryId = categories["Gaming"]
                    }
                };

                dbContext.Products.AddRange(products);
                dbContext.SaveChanges();
            }
        }

        private static void SeedProductImages(ApplicationDbContext dbContext)
        {
            if (!dbContext.ProductImages.Any())
            {
                // Get actual product IDs from database
                var productNameToIdMap = dbContext.Products.ToDictionary(p => p.Name, p => p.ProductId);

                var productImages = new ProductImage[]
                {
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/apple-macbook-pro-16-main.png",
                        ProductId = productNameToIdMap["MacBook Pro 16"]
                    },
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/dell-xps-15-main.png",
                        ProductId = productNameToIdMap["Dell XPS 15"]
                    },
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/apple-iphone-15-pro-main.png",
                        ProductId = productNameToIdMap["iPhone 15 Pro"]
                    },
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/samsung-galaxy-s23-ultra-main.jpeg",
                        ProductId = productNameToIdMap["Samsung Galaxy S23 Ultra"]
                    },
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/apple-magic-keyboard-main.png",
                        ProductId = productNameToIdMap["Magic Keyboard"]
                    },
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/logitech-mx-master-3-main.png",
                        ProductId = productNameToIdMap["Logitech MX Master 3"]
                    },
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/apple-airpods-pro-main.png",
                        ProductId = productNameToIdMap["AirPods Pro"]
                    },
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/sony-wh-1000xm5-main.jpg",
                        ProductId = productNameToIdMap["Sony WH-1000XM5"]
                    },
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/sony-playstation-5-main.png",
                        ProductId = productNameToIdMap["PlayStation 5"]
                    },
                    new ProductImage
                    {
                        // Remove explicit ProductImageId
                        Path = "/images/products/microsoft-xbox-series-x-main.png",
                        ProductId = productNameToIdMap["Xbox Series X"]
                    }
                };

                dbContext.ProductImages.AddRange(productImages);
                dbContext.SaveChanges();
            }
        }
    }
}