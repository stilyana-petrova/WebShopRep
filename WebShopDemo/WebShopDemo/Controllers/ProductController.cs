using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopDemo.Abstraction;
using WebShopDemo.Domain;
using WebShopDemo.Models.Brand;
using WebShopDemo.Models.Category;
using WebShopDemo.Models.Product;
using WebShopDemo.Services;

namespace WebShopDemo.Controllers
{
    [Authorize(Roles ="Administrator")]
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
        [AllowAnonymous]
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
        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            Product item = productService.GetProductById(id);
            if (item==null)
            {
                return NotFound();
            }
            ProductDetailsVM product = new ProductDetailsVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                BrandId = item.BrandId,
                BrandName = item.Brand.BrandName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };
            return View(product);
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
            Product product = productService.GetProductById(id);
            if (product==null)
            {
                return NotFound();
            }
            ProductEditVM updatedProduct = new ProductEditVM()
            {
                Id=product.Id,
                ProductName=product.ProductName,
                BrandId=product.BrandId,
                CategoryId=product.CategoryId,
                Picture=product.Picture,
                Quantity=product.Quantity,
                Price=product.Price,
                Discount=product.Discount
            };
            updatedProduct.Brands = brandService.GetBrands().Select(b => new BrandPairVM()
            {
                Id = b.Id,
                Name = b.BrandName
            }).ToList();
            updatedProduct.Categories = categortService.GetCategoties().Select(c => new CategoryPairVM()
            {
                Id = c.Id,
                Name = c.CategoryName
            }).ToList();
            return View(updatedProduct);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductEditVM product)
        {
            if (ModelState.IsValid)
            {
                var updated = productService.Update(id, product.ProductName,
                    product.BrandId, product.CategoryId, product.Picture,
                    product.Quantity, product.Price, product.Discount);
                if (updated)
                {
                    return this.RedirectToAction("Index");
                }
            }
            return View(product);
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            Product item = productService.GetProductById(id);
            if (item==null)
            {
                return NotFound();
            }
            ProductDeleteVM product = new ProductDeleteVM()
            {
                Id = item.Id,
                ProductName = item.ProductName,
                BrandId = item.BrandId,
                BrandName = item.Brand.BrandName,
                CategoryId = item.CategoryId,
                CategoryName = item.Category.CategoryName,
                Picture = item.Picture,
                Quantity = item.Quantity,
                Price = item.Price,
                Discount = item.Discount
            };
            return View();
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var deleted = productService.RemoveById(id);
            if (deleted)
            {
                return this.RedirectToAction("Success");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Success()
        {
            return View();
        }
    }
}
