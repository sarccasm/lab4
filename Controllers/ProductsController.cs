using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductsValidation.Models;
using ProductsValidation.Services;

namespace ProductsValidation.Controllers
{
    
    public class ProductsController : Controller
    {
        private List<Product> myProducts;

        public ProductsController(Data data)
        {
            myProducts = data.Products;
        }
        
        public IActionResult Index()
        {
            return View(myProducts);
        }
        
        public IActionResult View(int id)
        {
            Product prod = myProducts.Find(prod => prod.Id == id);
            if (prod != null)
            {
                return View(prod);
            }

            return View("NotExists");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Product prod = myProducts.Find(prod => prod.Id == id);
            if (prod != null)
            {
                SetProductTypes();
                return View(prod);
            }

            return View("NotExists");
        } 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Type,Name,Description,Price")] Product product)
        {
            Product existingProduct = myProducts.Find(prod => prod.Id == id);
            if (existingProduct == null)
            {
                return View("NotExists");
            }

            if (!ModelState.IsValid)
            {
                product.Id = id;
                SetProductTypes();
                return View(product);
            }

            existingProduct.Type = product.Type;
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            return RedirectToAction(nameof(View), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Type,Name,Description,Price")] Product product)
        {
            if (!ModelState.IsValid)
            {
                product.Id = GetNextProductId();
                SetProductTypes();
                return View(product);
            }

            product.Id = GetNextProductId();
            myProducts.Add(product);
            return RedirectToAction(nameof(View), new { id = product.Id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            SetProductTypes();
            return View(new Product { Id = GetNextProductId() });
        }

        public IActionResult Delete(int id)
        {
            Product product = myProducts.Find(item => item.Id == id);
            if (product != null)
            {
                myProducts.Remove(product);
            }

            return View("Index", myProducts);
        }

        [HttpGet]
        public IActionResult EditPrices(Product.Category category)
        {
            if (!Enum.IsDefined(typeof(Product.Category), category))
            {
                return BadRequest();
            }

            return View(new ProductPriceEditViewModel
            {
                Category = category,
                Products = GetProductsForCategory(category)
                    .Select(product => new ProductPriceEditItemViewModel
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price
                    })
                    .ToList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPrices(ProductPriceEditViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            List<Product> productsInCategory = Enum.IsDefined(typeof(Product.Category), model.Category)
                ? GetProductsForCategory(model.Category)
                : new List<Product>();

            bool submittedProductsMatchCategory =
                model.Products != null &&
                model.Products.Count == productsInCategory.Count &&
                model.Products.Select(item => item.Id).Distinct().Count() == model.Products.Count &&
                productsInCategory.All(product => model.Products.Any(item => item.Id == product.Id));

            if (!submittedProductsMatchCategory)
            {
                ModelState.AddModelError(string.Empty, "The submitted products do not match the selected category.");
            }
            if (!Enum.IsDefined(typeof(Product.Category), model.Category))
            {
                ModelState.AddModelError(nameof(model.Category), "Select a valid product category.");
            }

            if (!ModelState.IsValid)
            {
                PopulateProductNames(model, productsInCategory);
                return View(model);
            }

            foreach (ProductPriceEditItemViewModel item in model.Products)
            {
                Product product = productsInCategory.Find(existing => existing.Id == item.Id);
                product.Price = item.Price;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private int GetNextProductId()
        {
            return myProducts.Count == 0 ? 1 : myProducts.Max(product => product.Id) + 1;
        }

        private List<Product> GetProductsForCategory(Product.Category category)
        {
            return myProducts.Where(product => product.Type == category).ToList();
        }

        private void SetProductTypes()
        {
            ViewBag.ProductTypes = new SelectList(Enum.GetValues(typeof(Product.Category)));
        }

        private static void PopulateProductNames(
            ProductPriceEditViewModel model,
            IEnumerable<Product> productsInCategory)
        {
            if (model.Products == null)
            {
                model.Products = new List<ProductPriceEditItemViewModel>();
                return;
            }

            foreach (ProductPriceEditItemViewModel item in model.Products)
            {
                item.Name = productsInCategory
                    .FirstOrDefault(product => product.Id == item.Id)?.Name;
            }
        }
    }
}
