using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Wallet.Core.Constants;
using Wallet.Core.Contracts;
using Wallet.Core.Services;
using Wallet.Core.ViewModels.Asset;
using Wallet.Core.ViewModels.Category;
using Wallet.Infrastructure.Data.Models;
using Xunit;

namespace Wallet.Test.Services
{
    public class CategoryServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;
        private IRepository repo;
        private ICategoryService categoryService;

        [Fact]
        public void GetCategoryByIdShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var category = repo.All<Category>().First();

            Assert.NotNull(categoryService.GetCategoryById(category.Id));
            Assert.Equal(category,categoryService.GetCategoryById(category.Id));
            Assert.Null(categoryService.GetCategoryById(Guid.NewGuid()));
        }

        [Fact]
        public void CreateCategoryShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var category = repo.All<Category>().First();

            var modelExist = new CreateCategoryFormModel()
            {
                Name = category.Name,
                Description = category.Description
            };

            var modelCorrect = new CreateCategoryFormModel()
            {
                Name = "Stock",
                Description = "Unbelievable"
            };

            Assert.Equal((false, "Category exist!"), categoryService.Create(modelExist));
            Assert.Equal((true,null),categoryService.Create(modelCorrect));
        }

        [Fact]
        public void EditCategoryShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var category = repo.All<Category>().First();

            var modelNotExist = new EditCategoryModel()
            {
                Name = "c7",
                Description = "c9"
            };

            var modelCorrect = new EditCategoryModel()
            {
                CategoryId = category.Id,
                Name = "c8",
                Description = "c0"
            };

            Assert.Equal((false, "Invalid operation"),categoryService.Edit(modelNotExist));
            Assert.Equal((true,null),categoryService.Edit(modelCorrect));
        }

        [Fact]
        public void DeleteCategoryShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var category = repo.All<Category>().First();

            Assert.False(categoryService.Delete(Guid.NewGuid()));
            Assert.True(categoryService.Delete(category.Id));
        }

        [Fact]
        public void GetCategoryNameShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var category = repo.All<Category>().First();

            Assert.Equal(category.Name,categoryService.GetCategoryName(category.Id));
            Assert.Null(categoryService.GetCategoryName(Guid.NewGuid()));
        }

        [Fact]
        public void AssetCreateFormModelShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var category = repo.All<Category>().First();

            Assert.IsType<CreateAssetModel>(categoryService.AssetCreateFormModel(category.Id));
            Assert.NotNull(categoryService.AssetCreateFormModel(category.Id));
        }

        [Fact]
        public void GetAllCategoriesShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            Assert.NotEmpty(categoryService.GetAllCategories());
            Assert.IsType<List<AllCategoryViewModel>>(categoryService.GetAllCategories());
        }

        [Fact]
        public void GetDetailsOfCategoryShouldReturnCorrect()
        {
            GetInMemoryRepository();
            Seed(repo);

            var category = repo.All<Category>().First();

            Assert.NotNull(categoryService.GetDetailsOfCategory(category.Id));
            Assert.IsType<EditCategoryModel>(categoryService.GetDetailsOfCategory(category.Id));
        }


        private void GetInMemoryRepository()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IRepository, Repository>()
                .AddMemoryCache()
                .BuildServiceProvider();

            repo = serviceProvider.GetService<IRepository>();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();
            categoryService = new CategoryService(repo, memoryCache);
        }

        private void Seed(IRepository repo)
        {
            var category = new Category()
            {
                Name = "c1",
                Description = ""
            };

            var category2 = new Category()
            {
                Name = "c2",
                Description = ""
            };

            var category3 = new Category()
            {
                Name = "c3",
                Description = ""
            };

            repo.Add<Category>(category);
            repo.Add<Category>(category2);
            repo.Add<Category>(category3);
            repo.SaveChanges();
        }
    }
}
