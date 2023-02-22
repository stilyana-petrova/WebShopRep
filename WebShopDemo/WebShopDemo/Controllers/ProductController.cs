using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopDemo.Abstraction;
using WebShopDemo.Models.Brand;
using WebShopDemo.Models.Category;
using WebShopDemo.Models.Product;
using WebShopDemo.Services;

namespace WebShopDemo.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService; 
        private readonly ICategortService categortService;
        private readonly IBrandService brandService;

        public ProductController(IProductService productService, ICategortService categortService, IBrandService brandService)
        {
            this.productService = productService;
            this.categortService = categortService;
            this.brandService = brandService;
        }

        // GET: ProductController
        public ActionResult Index( string searchStringCategoryName, string searchBrandName)
        {
            List<ProductIndexVM> products = productService.GetProducts(searchStringCategoryName, searchBrandName)
                .Select(product => new ProductIndexVM
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    BrandId = product.BrandId,
                    BrandName = product.Brand.BrandName,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category.CategoryName,
                    Picture = product.Picture,
                    Quantity = product.Quantity,
                    Price = product.Price,
                    Discount = product.Discount

                }).ToList();
            return View(products);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            var product = new ProductCreateVM();
            product.Brands = brandService.GetBrands().Select(x => new BrandPairVM()
            {
                Id = x.Id,
                Name = x.BrandName
            }).ToList();
            product.Categories = categortService.GetCategoties().Select(x => new CategoryPairVM()
            {
                Id = x.Id,
                Name = x.CategoryName
            }).ToList();
            return View(product);
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] ProductCreateVM product)
        {
            if (ModelState.IsValid)
            {
                var createdId = productService.Create(product.ProductName, product.BrandId, product.CategoryId, product.Picture, product.Quantity, product.Price, product.Discount);
                if (createdId)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
