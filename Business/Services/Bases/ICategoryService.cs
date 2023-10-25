using AppCore.Business.Models.Results;
using AppCore.Business.Services.Bases;
using Business.Models;
using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services.Bases
{
    public interface ICategoryService : IService<CategoryModel, Category, BitirmeProjesiContext>
    {
        Task<Result<List<CategoryModel>>> GetCategoriesAsync();
    }
}
