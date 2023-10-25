using AppCore.DataAccess.EntityFramework.Bases;
using DataAccess.Contexts;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Bases
{
    public abstract class CategoryRepoBase : RepoBase<Category, BitirmeProjesiContext>
    {
        protected CategoryRepoBase() : base()
        {

        }

        protected CategoryRepoBase(BitirmeProjesiContext bitirmeProjesiContext) : base(bitirmeProjesiContext)
        {

        }
    }
}
