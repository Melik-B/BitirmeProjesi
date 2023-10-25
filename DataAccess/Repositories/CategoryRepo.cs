using DataAccess.Contexts;
using DataAccess.Repositories.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CategoryRepo : CategoryRepoBase
    {
        public CategoryRepo() : base()
        {

        }

        public CategoryRepo(BitirmeProjesiContext bitirmeProjesiContext) : base(bitirmeProjesiContext)
        {

        }
    }

}
