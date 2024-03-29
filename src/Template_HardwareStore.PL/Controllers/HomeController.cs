﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Template_HardwareStore.DAL.Repository.Interface;
using Template_HardwareStore.Entities.Models;
using Template_HardwareStore.Entities.Models.ViewModels;
using Template_HardwareStore.Utility.Constants;
using Template_HardwareStore.Utility.Extensions;

namespace Template_HardwareStore.PL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Products = _productRepository.GetAll(includeProperties: "ApplicationType,Category"),
                Categories = _categoryRepository.GetAll()
            };
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            };

            DetailViewModel detailViewModel = new DetailViewModel()
            {
                Product = _productRepository.FirstOrDefault(u => u.Id == id, includeProperties: "ApplicationType,Category"),
                ExistInCart = false,
            };

            foreach (var item in shoppingCartList)
            {
                if (item.ProductId == id)
                {
                    detailViewModel.ExistInCart = true;
                }
            }

            return View(detailViewModel);
        }

        [HttpPost, ActionName("AddToCart")]
        public IActionResult DetailsPost(int id, DetailViewModel detailViewModel)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
            };
            shoppingCartList.Add(new ShoppingCart { ProductId = id, SqFt = detailViewModel.Product.TempSqFt});

            HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            try
            {
                List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
                if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart) != null &&
                    HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WebConstants.SessionCart).Count() > 0)
                {
                    shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WebConstants.SessionCart);
                };

                var itemToRemove = shoppingCartList.SingleOrDefault(u => u.ProductId == id);
                if (itemToRemove != null)
                {
                    shoppingCartList.Remove(itemToRemove);
                }

                HttpContext.Session.Set(WebConstants.SessionCart, shoppingCartList);

                TempData[WebConstants.Success] = "Product remove from card seccessfully.";
            }
            catch (Exception ex)
            {
                TempData[WebConstants.Error] = $"Product remove from card with error {ex}!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
