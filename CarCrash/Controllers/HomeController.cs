﻿using CarCrash.Models;
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
        private ICarCrashRepository repo { get; set; }
        public HomeController(ICarCrashRepository temp)
        {
            repo = temp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Data()
        {
            var Crashes = repo.Crashes.ToList();
            List<Location> Locations = repo.Locations.ToList();
            List<int> locations = new List<string>();
            Crash crash;

            foreach (Crash c in Crashes) 
            {
                foreach (Location l in Locations)
                {
                    if (l.LOCATION_ID == c.LOCATION_ID)
                    {
                        locations.Add(l.LOCATION_ID);
                    }
                }
            }


            return View(Crashes);
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
