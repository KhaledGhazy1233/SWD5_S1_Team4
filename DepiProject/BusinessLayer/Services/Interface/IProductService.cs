using BusinessLayer.ViewModel.Product;

namespace BusinessLayer.Services.Interface;

public interface IProductService
{
    public List<GetProductDashboard> GetProductsVm();
    public Task<List<ProductForCategoryVm>> GetFeaturedProduct();
    public Task<UpdateProductVm> GetUpdateProductVmById(int productID);
    public GetProductDetails GetProductDetailsVm(int productID);
    public Task<List<ProductForCategoryVm>> GetProductByCategoryID(int categoryId);



    public Task<string> CreateAsync(CreateProductVm vm);
    public Task<string> UpdateAsync(UpdateProductVm vm);
    public Task<string> DeleteAsync(int productID);
}