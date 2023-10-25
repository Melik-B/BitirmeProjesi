using Business.Models.Filters;
using Business.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BitirmeProjesi.Models
{
    public class ProductsIndexViewModel
    {
        public List<ProductModel> Products { get; set; }
        public ProductFilterModel Filter { get; set; }
        public SelectList Categories { get; set; }
        public MultiSelectList Stores { get; set; }
        public int PageNumber { get; set; } = 1;
        public SelectList Pages { get; set; }
        public int TotalRecordCount { get; set; }
        public bool IsDirectionAscending { get; set; }
        public string Expression { get; set; }
        public SelectList SortBy { get; set; }
    }
}
