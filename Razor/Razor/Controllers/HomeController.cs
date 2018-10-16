﻿using Microsoft.AspNetCore.Mvc;
using Razor.Models;

namespace Razor.Controllers
{
    public class HomeController:Controller
    {
        //public ViewResult Index1()
        //{
        //    Product myProduct = new Product
        //    {
        //        ProductID = 1,
        //        Name = "Kayak",
        //        Description = "A boat for one person",
        //        Category = "Watersports",
        //        Price = 275M
        //    };

        //    ViewBag.StockLevel = 2;

        //    return View(myProduct);
        //}

        public IActionResult Index()
        {
            Product[] products =
            {
                new Product{Name="Kayak", Price=275M},
                new Product{Name="Lifejacket", Price=48.95M},
                new Product{Name="Soccer ball", Price=19.50M},
                new Product{Name="Corner flag", Price=34.95M}
            };

            return View(products);
        }
    }
}
