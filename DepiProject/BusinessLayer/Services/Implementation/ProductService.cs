using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Product;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
            Amount = vm.Amount,
            Brand = vm.Brand,
            CategoryId = vm.CategoryId,
            Price = vm.Price,
        };

        try
        {
            // begin transaction
            await _dbContext.Database.BeginTransactionAsync();
            // save    product
            _productRepository.Add(product);

            // then save image
            await saveNewFilesAsync(vm.Files, product.ProductId);

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

        // is new name exits exclude itself
        if (await _productRepository.IsProductNameExistExcludeItself(vm.Name, vm.ProductId))
            return "this new name is already exist";

        // get product
        var product = _productRepository.Get(p => p.ProductId == vm.ProductId);

        if (product == null)
            return "this product not exist";
        // map
        product.Price = vm.Price;
        product.CategoryId = vm.CategoryId;
        product.Name = vm.Name;
        product.Amount = vm.Amount;
        product.Description = vm.Description;
        product.Brand = vm.Brand;

        List<IFormFile> oldPaths = new List<IFormFile>();

        try
        {
            // start transaction
            await _dbContext.Database.BeginTransactionAsync();
            // if there is a image get old image then deleted then save new 

            await saveNewFilesAsync(vm.Files, product.ProductId);

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