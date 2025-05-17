using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Product;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BusinessLayer.Services.Implementation;

public class ProductService : IProductService
{
    #region   Fields
    private readonly IProductRepository _productRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly IFileService _fileService;
    #endregion

    #region   Constructor
    public ProductService(IProductRepository productRepository,
                          ApplicationDbContext dbContext,
                          IFileService fileService)
    {
        _productRepository = productRepository;
        _dbContext = dbContext;
        _fileService = fileService;
    }
    #endregion

    #region    Handle Methods
    public List<GetProductDashboard> GetProductsVm()
    {
        var products = _productRepository.GetAll(p => !p.IsDeleted, "Category");
        var response = new List<GetProductDashboard>();
        foreach (var product in products)
        {
            var vm = new GetProductDashboard()
            {

                IsAvailable = product.IsAvailable,
                Name = product.Name,
                Price = product.Price,
                ProductId = product.ProductId,
                StockQuantity = product.StockQuantity,

            };

            if (product.Category != null)
            {
                vm.CategoryNAme = product.Category.Name;
            }

            response.Add(vm);
        }

        return response;
    }
    public async Task<List<ProductForCategoryVm>> GetFeaturedProduct()
    {
        return await getProductByCategory(p => p.IsFeatured);
    }
    public async Task<List<ProductForCategoryVm>> GetProductByCategoryID(int categoryId)
    {
        return await getProductByCategory(p => p.CategoryId == categoryId);
    }
    private async Task<List<ProductForCategoryVm>> getProductByCategory(Expression<Func<Product, bool>> action)
    {
        var products = await _dbContext.Products
                                                  .Include(p => p.Category)
                                                  .Include(p => p.ProductImages)
                                                  .Where(action)
                                                  .ToListAsync();

        var response = new List<ProductForCategoryVm>();
        foreach (var product in products)
        {
            var vm = new ProductForCategoryVm()
            {
                Brand = product.Brand,
                CategoryName = product.Category.Name,
                ProductId = product.ProductId,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
            };
            if (product.ProductImages.Count > 0)
            {
                vm.ImageUrl = product.ProductImages!.FirstOrDefault().Path;
            }

            response.Add(vm);
        }

        return response;
    }

    public GetProductDetails GetProductDetailsVm(int productID)
    {
        var product = _productRepository.GetAll(p => !p.IsDeleted && p.ProductId == productID, "ProductImages").FirstOrDefault();

        var response = new GetProductDetails()
        {
            Brand = product.Brand,
            Description = product.Description,
            DiscountPercentage = product.DiscountPercentage,
            Model = product.Model,
            Name = product.Name,
            Price = product.Price,
            ProductId = productID,
            StockQuantity = product.StockQuantity,
            TechnicalSpecifications = product.TechnicalSpecifications,
            WarrantyInfo = product.WarrantyInfo,
        };

        if (product.ProductImages.FirstOrDefault() != null)
        {
            response.ImageUrl = product.ProductImages.FirstOrDefault()!.Path;
        }

        return response;
    }
    public Task<UpdateProductVm> GetProductByIdVm(int productID)
    {
        throw new NotImplementedException();
    }
    public Task<UpdateProductVm> GetUpdateProductVmById(int productID)
    {
        var produnt = _productRepository.Get(p => !p.IsDeleted && p.ProductId == productID, "Category");
        var categories = _dbContext.Categories.Where(c => !c.IsDeleted);
        var editVm = new UpdateProductVm()
        {
            StockQuantity = produnt.StockQuantity,
            Brand = produnt.Brand,
            Name = produnt.Name,
            Description = produnt.Description,
            Price = produnt.Price,
            ProductId = productID,
            DiscountPercentage = produnt.DiscountPercentage,
            IsFeatured = produnt.IsFeatured,
            TechnicalSpecifications = produnt.TechnicalSpecifications,
            WarrantyInfo = produnt.WarrantyInfo,
            Model = produnt.Model,
        };

        return Task.FromResult(editVm);
    }
    public async Task<string> CreateAsync(CreateProductVm vm)
    {
        // check if name exist
        if (await _productRepository.IsProductNameExist(vm.Name))
            return "this name is already exist";
        // map
        var product = new Product()
        {
            Name = vm.Name,
            Description = vm.Description,
            StockQuantity = vm.StockQuantity,
            Brand = vm.Brand,
            CategoryId = vm.CategoryId,
            Price = vm.Price,
            Model = vm.Model,
            DiscountPercentage = vm.DiscountPercentage,
            IsAvailable = true,
            IsFeatured = vm.IsFeatured,
            TechnicalSpecifications = vm.TechnicalSpecifications,
            WarrantyInfo = vm.WarrantyInfo,
        };

        try
        {
            // begin transaction
            await _dbContext.Database.BeginTransactionAsync();
            // save    product
            _productRepository.Add(product);

            // then save image
            var files = new List<IFormFile>();
            files.Add(vm.ImageUrl);
            await saveNewFilesAsync(files, product.ProductId);

            // commit
            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
            // return result
            return "Success";
        }
        catch (Exception ex)
        {
            await _dbContext.Database.CommitTransactionAsync();
            return "Unexpected error happen try later";
        }

    }
    public async Task<string> UpdateAsync(UpdateProductVm vm)
    {
        // get product
        var product = _productRepository.Get(p => p.ProductId == vm.ProductId);

        if (product == null)
            return "this product not exist";
        // map
        product.Price = vm.Price;
        product.CategoryId = vm.CategoryId;
        product.Name = vm.Name;
        product.StockQuantity = vm.StockQuantity;
        product.Description = vm.Description;
        product.Brand = vm.Brand;
        product.WarrantyInfo = vm.WarrantyInfo;
        product.DiscountPercentage = vm.DiscountPercentage;
        product.Model = vm.Model;
        product.IsFeatured = vm.IsFeatured;
        product.TechnicalSpecifications = vm.TechnicalSpecifications;


        List<IFormFile> oldPaths = new List<IFormFile>();

        try
        {
            // start transaction
            await _dbContext.Database.BeginTransactionAsync();
            // if there is a image get old image then deleted then save new 
            var files = new List<IFormFile>();
            files.Add(vm.ImageUrl);
            await saveNewFilesAsync(files, product.ProductId);

            // then update product
            await _productRepository.Update(product);
            await _dbContext.SaveChangesAsync();

            await DeleteOldFilesAsync(product.ProductId);
            // then finish
            await _dbContext.Database.CommitTransactionAsync();
            return "Success";
        }
        catch (Exception ex)
        {
            await _dbContext.Database.RollbackTransactionAsync();
            return $"Error Happen => {ex.Message}";
        }
    }
    public async Task<string> DeleteAsync(int productID)
    {
        var product = _productRepository.Get(p => p.ProductId == productID);

        if (product == null)
            return "this product not exist";

        product.IsDeleted = true;

        await _productRepository.Update(product);
        return "Success";
    }

    private async Task saveNewFilesAsync(List<IFormFile> Files, int productId)
    {

        foreach (var file in Files)
        {
            var path = await _fileService.UploadFileAsync(file);

            var productImage = new ProductImage()
            {
                Path = path,
                ProductId = productId,
            };
            await _dbContext.ProductImages.AddAsync(productImage);
        }
    }
    private async Task DeleteOldFilesAsync(int productId)
    {
        var oldProductImages = await _dbContext.ProductImages
                                                              .Where(pimg => pimg.ProductId == productId)
                                                              .ToListAsync();
        foreach (var oldProductImage in oldProductImages)
        {
            // delete physical
            await _fileService.DeleteImageByUrlAsync(oldProductImage.Path);
        }
    }
    #endregion
}