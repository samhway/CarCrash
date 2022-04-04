using CarCrash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(CrashDbContext temp)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Data()
        {
            return View();
        }
        public IActionResult Prediction()
        {
            return View();
        }
        public IActionResult Summary()
        {
            return View();
        }
    }
}
