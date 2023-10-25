using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.DataAccess.EntityFramework;
using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using Business.Models.Filters;
using DataAccess.Contexts;

namespace Business.Services
{
    public interface IProductService : IService<ProductModel, Product, BitirmeProjesiContext>
    {
        #region Pagination
        int GetTotalRecordsCount(ProductFilterModel filter);
        Result<List<ProductModel>> List(ProductFilterModel filter, int pageNo, int recordsPerPage, string expression, bool isDirectionAscending);
        List<int> GetPages(int totalRecords, int recordsPerPage);
        List<string> GetExpressions();
        #endregion
        void DeleteImage(int id);
    }

    public class ProductService : IProductService
    {
        BitirmeProjesiContext _dbContext;
        public RepoBase<Product, BitirmeProjesiContext> Repo { get; set; }
        private RepoBase<ProductStore, BitirmeProjesiContext> _productStoreRepo;
        RepoBase<Store, BitirmeProjesiContext> _storeRepo;
        RepoBase<Category, BitirmeProjesiContext> _categoryRepo;

        public ProductService()
        {
            _dbContext = new BitirmeProjesiContext();
            Repo = new Repo<Product, BitirmeProjesiContext>(_dbContext);
            _productStoreRepo = new Repo<ProductStore, BitirmeProjesiContext>(_dbContext);
            _storeRepo = new Repo<Store, BitirmeProjesiContext>(_dbContext);
            _categoryRepo = new Repo<Category, BitirmeProjesiContext>(_dbContext);
        }

        public Result Add(ProductModel model)
        {
            if (Repo.Query().Any(p => p.Name.ToLower() == model.Name.ToLower().Trim()))
                return new ErrorResult("A record with the specified product name already exists!");
            if (model.ExpirationDate.HasValue && model.ExpirationDate.Value < DateTime.Today)
                return new ErrorResult("The expiration date should be today or later!");
            Product entity = new Product()
            {
                Description = model.Description?.Trim(),
                Name = model.Name.Trim(),
                UnitPrice = model.UnitPrice.Value,
                CategoryId = model.CategoryId.Value,
                ExpirationDate = model.ExpirationDate,
                StockQuantity = model.StockQuantity.Value,
                ProductStores = model.StoreIds?.Select(storeId => new ProductStore()
                {
                    StoreId = storeId
                }).ToList(),
                Image = model.Image,
                ImageExtension = model.ImageExtension?.ToLower()
            };

            Repo.Add(entity);
            model.Id = entity.Id;
            return new SuccessResult("Product added successfully.");
        }

        public Result Delete(int id)
        {
            var product = Repo.Query("ProductStores").SingleOrDefault(p => p.Id == id);
            if (product.ProductStores != null && product.ProductStores.Count > 0)
            {
                foreach (var productStore in product.ProductStores)
                {
                    _productStoreRepo.Delete(productStore, false);
                }
                _productStoreRepo.Save();
            }
            Repo.Delete(p => p.Id == id);
            return new SuccessResult("Product deleted successfully!");
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<ProductModel> Query()
        {
            // Use AutoMapper
            var productQuery = Repo.Query();
            var productStoreQuery = _productStoreRepo.Query();
            var storeQuery = _storeRepo.Query();
            var categoryQuery = _categoryRepo.Query();

            // Left Outer Join
            var query = from product in productQuery
                        join productStore in productStoreQuery
                        on product.Id equals productStore.ProductId into productStores
                        from subProductStores in productStores.DefaultIfEmpty()
                        join store in storeQuery
                        on subProductStores.StoreId equals store.Id into stores
                        from subStores in stores.DefaultIfEmpty()
                        join category in categoryQuery
                        on product.CategoryId equals category.Id into categories
                        from subCategories in categories.DefaultIfEmpty()
                        select new ProductModel()
                        {
                            Id = product.Id,
                            Description = product.Description,
                            Name = product.Name,
                            UnitPrice = product.UnitPrice,
                            CategoryId = product.CategoryId,
                            ExpirationDate = product.ExpirationDate,
                            StockQuantity = product.StockQuantity,
                            UnitPriceDisplay = product.UnitPrice.ToString("C2"),
                            ExpirationDateDisplay = product.ExpirationDate.HasValue ? product.ExpirationDate.Value.ToString("yyyy-MM-dd") : "",
                            CategoryNameDisplay = subCategories.Name,
                            StoreNameDisplay = subStores != null ? subStores.Name : "",
                            StoreId = subStores != null ? subStores.Id : 0,
                            Image = product.Image,
                            ImageExtension = product.ImageExtension,
                            ImgSrcDisplay = product.Image != null ? ((product.ImageExtension == ".jpg" || product.ImageExtension == ".jpeg" ? "data:image/jpeg;base64," : "data:image/png;base64,") + Convert.ToBase64String(product.Image)) : null
                        };
            return query;
        }

        public Result Update(ProductModel model)
        {
            if (Repo.Query().Any(p => p.Name.ToLower() == model.Name.ToLower().Trim() && p.Id != model.Id))
                return new ErrorResult("A record with the specified product name already exists!");
            if (model.ExpirationDate.HasValue && model.ExpirationDate.Value < DateTime.Today)
                return new ErrorResult("The expiration date should be today or later!");
            Product entity = Repo.Query(p => p.Id == model.Id, "ProductStores").SingleOrDefault();
            if (entity.ProductStores != null && entity.ProductStores.Count > 0)
            {
                foreach (var productStore in entity.ProductStores)
                {
                    _productStoreRepo.Delete(productStore, false);
                }
                _productStoreRepo.Save();
            }
            entity.Name = model.Name.Trim();
            entity.Description = model.Description?.Trim();
            entity.UnitPrice = model.UnitPrice.Value;
            entity.CategoryId = model.CategoryId.Value;
            entity.ExpirationDate = model.ExpirationDate;
            entity.StockQuantity = model.StockQuantity.Value;
            entity.ProductStores = model.StoreIds?.Select(storeId => new ProductStore()
            {
                StoreId = storeId
            }).ToList();
            if (model.Image != null)
            {
                entity.Image = model.Image;
                entity.ImageExtension = model.ImageExtension.ToLower();
            }
            Repo.Update(entity);
            return new SuccessResult();
        }

        #region Pagination
        private IQueryable<ProductModel> Query(ProductFilterModel filter)
        {
            var query = Query();
            if (filter != null)
            {
                if (filter.CategoryId.HasValue)
                    query = query.Where(q => q.CategoryId == filter.CategoryId.Value);
                if (!string.IsNullOrWhiteSpace(filter.ProductName))
                    query = query.Where(q => q.Name.ToUpper().Contains(filter.ProductName.ToUpper().Trim()));
                if (filter.UnitPriceStart != null)
                    query = query.Where(q => q.UnitPrice >= filter.UnitPriceStart.Value);
                if (filter.UnitPriceEnd.HasValue)
                    query = query.Where(q => q.UnitPrice <= filter.UnitPriceEnd.Value);
                if (filter.StockQuantityStart != null)
                    query = query.Where(q => q.StockQuantity >= filter.StockQuantityStart.Value);
                if (filter.StockQuantityEnd.HasValue)
                    query = query.Where(q => q.StockQuantity <= filter.StockQuantityEnd.Value);
                if (filter.StoreIds != null && filter.StoreIds.Count > 0)
                    query = query.Where(q => filter.StoreIds.Contains(q.StoreId));
            }
            return query;
        }

        public Result<List<ProductModel>> List(ProductFilterModel filter, int pageNo, int recordsPerPage, string expression, bool isDirectionAscending)
        {
            var query = Query(filter);

            switch (expression)
            {
                case "Product Name":
                    query = isDirectionAscending ? query.OrderBy(q => q.Name) : query.OrderByDescending(q => q.Name);
                    break;
                case "Unit Price":
                    query = isDirectionAscending ? query.OrderBy(q => q.UnitPrice) : query.OrderByDescending(q => q.UnitPrice);
                    break;
                default:
                    query = isDirectionAscending ? query.OrderBy(q => q.ExpirationDate) : query.OrderByDescending(q => q.ExpirationDate);
                    break;
            }
            query = query.Skip((pageNo - 1) * recordsPerPage).Take(recordsPerPage);

            var list = query.ToList();
            return new SuccessResult<List<ProductModel>>(list);
        }

        public int GetTotalRecordsCount(ProductFilterModel filter)
        {
            return Query(filter).Count();
        }

        public List<int> GetPages(int totalRecords, int recordsPerPage)
        {
            List<int> pages = new List<int>();
            int totalPages = (int)Math.Ceiling((double)totalRecords / (double)recordsPerPage);
            for (int page = 1; page <= totalPages; page++)
            {
                pages.Add(page);
            }
            return pages;
        }

        public void DeleteImage(int id)
        {
            var entity = Repo.Query().SingleOrDefault(p => p.Id == id);
            entity.Image = null;
            entity.ImageExtension = null;
            Repo.Update(entity);
        }
        #endregion
        public List<string> GetExpressions() => new List<string>()
        {
        "Product Name", "Unit Price", "Expiration Date"
        };
    }
}
