using BusinessLayer.ViewModel.Product;

namespace BusinessLayer.Services.Interface;

public interface IProductService
{
    public Task<string> CreateAsync(CreateProductVm vm);
    public Task<string> UpdateAsync(UpdateProductVm vm);
    public Task<string> DeleteAsync(int productID);
}