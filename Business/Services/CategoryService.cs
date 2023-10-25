using AppCore.Business.Models.Results;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.DataAccess.EntityFramework;
using Business.Models;
using Business.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using DataAccess.Contexts;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {
        public RepoBase<Category, BitirmeProjesiContext> Repo { get; set; } = new Repo<Category, BitirmeProjesiContext>();

        public Result Add(CategoryModel model)
        {
            if (Repo.Query().Any(c => c.Name.ToUpper() == model.Name.ToLower().Trim()))
                return new ErrorResult("A record with the entered category name already exists!");

            Category entity = new Category()
            {
                Name = model.Name.Trim(),
                Description = model.Description?.Trim()
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            Category entity = Repo.Query(c => c.Id == id, "Products").SingleOrDefault();
            if (entity.Products != null && entity.Products.Count > 0)
            {
                return new ErrorResult("The category cannot be deleted as it has associated products!");
            }
            Repo.Delete(entity);
            return new SuccessResult("Category deleted successfully.");
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<CategoryModel> Query()
        {
            IQueryable<CategoryModel> query = Repo.Query("Products").OrderBy(c => c.Name).Select(c => new CategoryModel()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ProductCountDisplay = c.Products.Count
            });
            return query;
        }

        public Result Update(CategoryModel model)
        {
            if (Repo.Query().Any(category => category.Name.ToUpper() == model.Name.ToUpper().Trim() && category.Id != model.Id))
                return new ErrorResult("A record with the entered category name already exists!");

            Category entity = Repo.Query(c => c.Id == model.Id).SingleOrDefault();
            entity.Name = model.Name.Trim();
            entity.Description = model.Description?.Trim();
            Repo.Update(entity);
            return new SuccessResult("Category updated successfully.");
        }

        public async Task<Result<List<CategoryModel>>> GetCategoriesAsync()
        {
            List<CategoryModel> categories = await Query().ToListAsync();
            return new SuccessResult<List<CategoryModel>>(categories);
        }
    }
}
