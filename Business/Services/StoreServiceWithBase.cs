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
using static System.Formats.Asn1.AsnWriter;
using DataAccess.Contexts;
using DataAccess.Entities;

namespace Business.Services
{
    public interface IStoreService : IService<StoreModel, Store, BitirmeProjesiContext>
    {
    }

    public class StoreService : IStoreService
    {
        public RepoBase<Store, BitirmeProjesiContext> Repo { get; set; } = new Repo<Store, BitirmeProjesiContext>();
        RepoBase<ProductStore, BitirmeProjesiContext> _productStoreRepo = new Repo<ProductStore, BitirmeProjesiContext>();

        public Result Add(StoreModel model)
        {
            if (Repo.Query().Any(s => s.Name.ToLower() == model.Name.ToLower().Trim()))
                return new ErrorResult("A record with the entered store name already exists!");

            Store entity = new Store()
            {
                Name = model.Name.Trim(),
                Rating = model.Rating,
                IsVirtual = model.IsVirtual
            };
            Repo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            var store = Repo.Query("ProductStores").SingleOrDefault(s => s.Id == id);
            if (store.ProductStores != null && store.ProductStores.Count > 0)
            {
                foreach (var productStore in store.ProductStores)
                {
                    _productStoreRepo.Delete(productStore, false);
                }
                _productStoreRepo.Save();
            }
            Repo.Delete(s => s.Id == id);
            return new SuccessResult();
        }

        public void Dispose()
        {
            Repo.Dispose();
        }

        public IQueryable<StoreModel> Query()
        {
            return Repo.Query().OrderByDescending(s => s.IsVirtual).ThenBy(s => s.Name).Select(s => new StoreModel()
            {
                Id = s.Id,
                Name = s.Name,
                Rating = s.Rating,
                IsVirtual = s.IsVirtual,
                IsVirtualDisplay = s.IsVirtual ? "Yes" : "No"
            });
        }

        public Result Update(StoreModel model)
        {
            if (Repo.Query().Any(s => s.Name.ToLower() == model.Name.ToLower().Trim() && s.Id != model.Id))
                return new ErrorResult("A record with the entered store name already exists!");

            Store entity = Repo.Query(s => s.Id == model.Id).SingleOrDefault();
            entity.Name = model.Name.Trim();
            entity.Rating = model.Rating;
            entity.IsVirtual = model.IsVirtual;
            Repo.Update(entity);
            return new SuccessResult();
        }
    }
}