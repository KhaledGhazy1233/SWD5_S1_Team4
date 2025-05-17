using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Category;
using BusinessLayer.Wrapper;
using DataLayer.Entities;
using DataLayer.Repository.IRepository;

namespace BusinessLayer.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFileService _fileService;


        public CategoryService(ICategoryRepository categoryRepository, IFileService fileService)
        {
            _categoryRepository = categoryRepository;
            _fileService = fileService;
        }

        public async Task<string> Create(CreateCategoryVm vm)
        {
            if (await _categoryRepository.IsCategoryNameExist(vm.Name))
                return "This name already exists";

            var path = await _fileService.UploadFileAsync(vm.ImageUrl);
            var category = new Category()
            {
                Name = vm.Name,
                Description = vm.Description,
                IsDeleted = false,
                ImageUrl = path,
            };

            _categoryRepository.Add(category);
            await _categoryRepository.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> Update(UpdateCategoryVm vm)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(vm.Id);
            if (category == null)
                return "Category not found";

            var newNAmeExist = await _categoryRepository.IsCategoryNameExistExcludeItself(vm.Name, vm.Id);
            if (newNAmeExist)
                return "this new name is already exsit";


            if (vm.ImageUrl != null)
            {
                await _fileService.DeleteImageByUrlAsync(category.ImageUrl);
                var path = await _fileService.UploadFileAsync(vm.ImageUrl);

                category.ImageUrl = path;
            }


            category.Name = vm.Name;
            category.Description = vm.Description;

            await _categoryRepository.SaveChangesAsync();

            return "Success";
        }

        public async Task<string> Delete(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
                return "Category not found";

            await _categoryRepository.SoftDeleteAsync(id);

            return "Success";
        }

        public async Task<List<ShowAllCategoryVm>> GetAllCategories()
        {
            var result = await _categoryRepository.GetAllCategoriesIncludeProductsAsync();

            var response = new List<ShowAllCategoryVm>();
            foreach (var category in result)
            {
                var vm = new ShowAllCategoryVm()
                {
                    Description = category.Description,
                    Id = category.CategoryId,
                    Name = category.Name,
                    ProductCount = category.Products.Count,
                };
                response.Add(vm);
            }

            return response;
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }
        //last
        public async Task<UpdateCategoryVm?> GetUpdateCategoryVmById(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
                return null;

            return new UpdateCategoryVm
            {
                Id = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<List<ShowAllCategoryVm>?> GetPaginatedCategories(int pageNumber, int pageSize)
        {
            PaginateResult<Category> result = await _categoryRepository.GetQuerableCategories()
                                                                     .ToPaginatedListAsync(pageNumber, pageSize);

            var response = new List<ShowAllCategoryVm>();
            if (result.Data == null || result.Data.Count <= 0)
            {
                return null;
            }

            foreach (var category in result.Data)
            {
                var vm = new ShowAllCategoryVm()
                {
                    Description = category.Description,
                    Id = category.CategoryId,
                    Name = category.Name,
                    ProductCount = category.Products.Count,
                };
                response.Add(vm);
            }
            return response;
        }

        public List<CategoryDropDown> GetDropDown()
        {
            var result = _categoryRepository.GetAll(p => p.IsDeleted == false);

            var response = new List<CategoryDropDown>();

            foreach (var category in result)
            {
                var vm = new CategoryDropDown()
                {
                    Id = category.CategoryId,
                    Name = category.Name,
                };
                response.Add(vm);
            }

            return response;
        }
    }
}


