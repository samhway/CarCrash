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
            List<Road> Roads = repo.Roads.ToList();
            Dictionary<int, string> locations = new Dictionary<int, string>();
            Dictionary<int, string> roads = new Dictionary<int, string>();

            foreach (Crash c in Crashes)
            {
                foreach (Location l in Locations)
                {
                    if (l.LOCATION_ID == c.LOCATION_ID)
                    {
                        locations[l.LOCATION_ID] =  l.CITY + ", " + l.COUNTY_NAME;
                    }
                }
            }

            foreach (Crash c in Crashes)
            {
                foreach (Road r in Roads)
                {
                    if (r.ROAD_ID == c.ROAD_ID)
                    {
                        roads[r.ROAD_ID] = r.MAIN_ROAD_NAME;
                    }
                }
            }

            ViewBag.locations = locations;
            ViewBag.roads = roads;
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

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.loc = repo.Locations.ToList();
            ViewBag.road = repo.Roads.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Crash c)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Data");
            }
            else
            {
                return View();
            }

        }
    }
}
