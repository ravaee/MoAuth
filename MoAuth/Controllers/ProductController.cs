using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakweenTemplate.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Product.Create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "Product.View")]
        public IActionResult Details(int? id)
        {
            return View();
        }

        [Authorize(Policy = "Product.Edit")]
        public IActionResult Edit()
        {
            return View();
        }

        [Authorize(Policy = "Product.Delete")]
        public IActionResult Delete()
        {
            return View();
        }

    }
}
