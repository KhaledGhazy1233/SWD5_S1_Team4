using BusinessLayer.ViewModel.Home;

namespace BusinessLayer.Services.Interface;

public interface ISharedService
{
    public HomeVm GetHomeDetailsAsync();
}