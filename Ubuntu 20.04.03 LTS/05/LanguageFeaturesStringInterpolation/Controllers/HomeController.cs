// using Microsoft.AspNetCore.Mvc;
// using LanguageFeatures.Models;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        /// Adds "clutter"
        // bool FilterByPrice(Product? p)
        // {
        //     return (p?.Price ?? 0) >= 20;
        // }

        public ViewResult Index()
        {
            IProductSelection cart = new ShoppingCart(
            new Product { Name = "Kayak", Price = 275M },
            new Product { Name = "Lifejacket", Price = 48.95M },
            new Product { Name = "Soccer ball", Price = 19.50M },
            new Product { Name = "Corner flag", Price = 34.95M }
            );
            return View(cart.Names);
        }

        // public ViewResult Index() => View(Product.GetProducts().Select(p => p?.Name));
        // {
        // return View(Product.GetProducts().Select(p => p?.Name));

        // ShoppingCart cart = new() { Products = Product.GetProducts() };

        // Product[] productArray = {
        //     new Product { Name = "Kayak", Price = 275M },
        //     new Product { Name = "LifeJacket", Price = 48.95M },
        //     new Product { Name = "Soccer ball", Price = 19.50M },
        //     new Product { Name = "Corner flag", Price = 34.95M }
        // };

        /// "Awkward" syntax
        // Func<Product?, bool> nameFilter = delegate (Product? prod)
        // {
        //     return prod?.Name?[0] == 'S';
        // };

        // decimal priceFilterTotal = productArray
        //     //.Filter(FilterByPrice)
        //     .Filter(p => (p?.Price ?? 0) >= 20)
        //     .TotalPrices();

        // decimal nameFilterTotal = productArray
        //     .Filter(p => (p?.Name?[0] == 'S'))
        //     .TotalPrices();

        // return View("Index", new string[] {
        //     $"Price Total: {priceFilterTotal:C2}",
        //     $"Name Total: {nameFilterTotal:C2}" });
        // }
    }
}