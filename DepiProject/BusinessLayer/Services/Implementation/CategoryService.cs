using BusinessLayer.Services.Interface;
using BusinessLayer.ViewModel.Category;
using DataLayer.Entities;
using DataLayer.Repository;
using DataLayer.Repository.IRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<string> Create(CreateCategoryVm vm)
        {
            if (await _categoryRepository.IsCategoryNameExist(vm.Name))
                return "This name already exists";

            var category = new Category()
            {
                Name = vm.Name,
                Description = vm.Description,
                IsDeleted = false
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

        public async Task<List<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
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

    }
}


