using DepiProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DepiProject.Controllers
{
    public class ProductController1 : Controller
    {
        // Sample data for demonstration - in a real app this would come from a database
        private static List<Product> _products = new List<Product>
        {            // Laptops
            new Product {
                Id = 1,
                Name = "TechBook Pro",
                Description = "Powerful laptop with high-performance features for professionals.",
                Price = 1299.99m,
                IsAvailable = true,
                ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.okPHK-lOk_E5nzOZsGx2dwHaFI?cb=iwc2&rs=1&pid=ImgDetMain",
                CategoryId = 1,
                Brand = "TechXpress",
                Model = "TB-2023",
                TechnicalSpecifications = "Intel Core i7, 16GB RAM, 512GB SSD, 15.6\" 4K Display",
                DiscountPercentage = 5,
                IsFeatured = true,
                WarrantyInfo = "2 Years Limited Warranty",
                StockQuantity = 15
            },
            new Product {
                Id = 2,
                Name = "GamerStation X",
                Description = "Ultimate gaming laptop with top-tier graphics and cooling system.",
                Price = 1899.99m,
                IsAvailable = true,
                ImageUrl = "https://tse4.mm.bing.net/th/id/OIP.okPHK-lOk_E5nzOZsGx2dwHaFI?cb=iwc2&rs=1&pid=ImgDetMain",
                CategoryId = 1,
                Brand = "TechXpress",
                Model = "GS-X500",
                TechnicalSpecifications = "AMD Ryzen 9, 32GB RAM, 1TB SSD, NVIDIA RTX 3080, 17.3\" 144Hz Display",
                DiscountPercentage = 0,
                IsFeatured = true,
                WarrantyInfo = "3 Years Premium Warranty",
                StockQuantity = 8
            },
              // Mobiles
            new Product {
                Id = 3,
                Name = "UltraPhone 12",
                Description = "Flagship smartphone with advanced camera system and all-day battery.",
                Price = 899.99m,
                IsAvailable = true,
                ImageUrl = "https://purepng.com/public/uploads/large/purepng.com-mobile-phone-with-touchmobilemobile-phonehandymobile-devicetouchscreenmobile-phone-device-231519333033crymn.png",
                CategoryId = 2,
                Brand = "UltraTech",
                Model = "UP-12",
                TechnicalSpecifications = "6.7\" OLED Display, 12GB RAM, 256GB Storage, 5000mAh Battery",
                DiscountPercentage = 10,
                IsFeatured = true,
                WarrantyInfo = "1 Year Manufacturer Warranty",
                StockQuantity = 25
            },
            new Product {
                Id = 4,
                Name = "SlimTab Pro",
                Description = "Ultra-thin tablet perfect for productivity and entertainment on the go.",
                Price = 649.99m,
                IsAvailable = true,
                ImageUrl = "https://purepng.com/public/uploads/large/purepng.com-mobile-phone-with-touchmobilemobile-phonehandymobile-devicetouchscreenmobile-phone-device-231519333033crymn.png",
                CategoryId = 2,
                Brand = "SlimTech",
                Model = "ST-P10",
                TechnicalSpecifications = "10.5\" Retina Display, 8GB RAM, 128GB Storage, 12MP Camera",
                DiscountPercentage = 0,
                IsFeatured = false,
                WarrantyInfo = "1 Year Limited Warranty",
                StockQuantity = 12
            },
            
            // Cameras
            new Product {
                Id = 5,
                Name = "ProShot DSLR X200",                Description = "Professional DSLR camera with exceptional image quality and versatile lens options.",
                Price = 1499.99m,
                IsAvailable = true,
                ImageUrl = "https://tse3.mm.bing.net/th/id/OIP.Kuw2tT6BMjFfN3UpgO1j2QHaE8?cb=iwc2&rs=1&pid=ImgDetMain",
                CategoryId = 3,
                Brand = "ProShot",
                Model = "X200",
                TechnicalSpecifications = "24.2MP Full-Frame Sensor, 4K Video, 45-Point AF System, ISO 100-25600",
                DiscountPercentage = 7,
                IsFeatured = true,
                WarrantyInfo = "2 Years International Warranty",
                StockQuantity = 5
            },
            new Product {
                Id = 6,
                Name = "MirrorLite M50",
                Description = "Compact mirrorless camera offering the perfect balance of quality and portability.",
                Price = 799.99m,
                IsAvailable = true,
                ImageUrl = "https://tse3.mm.bing.net/th/id/OIP.Kuw2tT6BMjFfN3UpgO1j2QHaE8?cb=iwc2&rs=1&pid=ImgDetMain",
                CategoryId = 3,
                Brand = "CameraElite",
                Model = "ML-M50",
                TechnicalSpecifications = "20.1MP APS-C Sensor, 4K/30p Video, 3\" Vari-angle Touchscreen",
                DiscountPercentage = 0,
                IsFeatured = false,
                WarrantyInfo = "1 Year Manufacturer Warranty",
                StockQuantity = 10
            }
        };

        public IActionResult Index()
        {
            return View(_products);
        }

        public IActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Get products by category
        public IActionResult ByCategory(int categoryId)
        {
            var products = _products.Where(p => p.CategoryId == categoryId).ToList();
            return View("Index", products);
        }

        // Get featured products
        public IActionResult Featured()
        {
            var products = _products.Where(p => p.IsFeatured).ToList();
            return View("Index", products);
        }
    }
}
