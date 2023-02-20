using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopDemo.Abstraction;
using WebShopDemo.Data;
using WebShopDemo.Domain;

namespace WebShopDemo.Services
{
    public class CategoryService:ICategortService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Category GetCategoryById(int categoryId)
        {
            return _context.Categories.Find(categoryId);
        }

        public List<Category> GetCategoties()
        {
            List<Category> categories = _context.Categories.ToList();
            return categories;
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return _context.Products.Where(x => x.CategoryId == categoryId).ToList();
        }
    }
}
