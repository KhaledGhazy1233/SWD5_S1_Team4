using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Home;
using DataLayer.Context;
using DataLayer.Repository.IRepository;

namespace BusinessLayer.Services.Implementation;

public class SharedService : ISharedService
{
    #region   Fields
    public ApplicationDbContext _dbContext { get; set; }
    public IUnitOfWork _unitOfWork { get; set; }
    #endregion

    #region   Constructor
    public SharedService(ApplicationDbContext applicationDbContext, IUnitOfWork unitOfWork)
    {
        _dbContext = applicationDbContext;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region   Methods
    public HomeVm GetHomeDetailsAsync()
    {
        var response = new HomeVm();

        var featuredProduct = _unitOfWork.Product.GetAll(p => p.IsFeatured && !p.IsDeleted, "ProductImages");
        var categoryWithMaxNumberOfProduct = _unitOfWork.Category.GetAll(c => !c.IsDeleted && c.ImageUrl != null, "Products");

        foreach (var product in featuredProduct)
        {
            var productVm = new HomeProduct()
            {
                Brand = product.Brand,
                Description = product.Description,
                DiscountPercentage = product.DiscountPercentage,
                Model = product.Model,
                Name = product.Name,
                Price = product.Price,
                ProductId = product.ProductId,
                StockQuantity = product.StockQuantity,
            };
            if (product.ProductImages.Count > 0)
            {
                productVm.Path = product.ProductImages.FirstOrDefault().Path;
            }
            response.Products.Add(productVm);
        }

        foreach (var category in categoryWithMaxNumberOfProduct)
        {
            var categoryVm = new HomeCategory()
            {
                Name = category.Name,
                Description = category.Description,
                CategoryId = category.CategoryId,
                ImageUrl = category.ImageUrl,
                ProductCount = category.Products.Count,
            };

            response.Category.Add(categoryVm);
        }

        return response;

    }
    //var result = _dbContext.Categories
    //                                                .Include(c => c.Products)
    //                                                .GroupBy(c => c.CategoryId);


    //    foreach (var category in result)
    //    {
    //        foreach (var item in category)
    //        {

    //        }
    //    }
    #endregion
}