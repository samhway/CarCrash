using CarCrash.Models;
using CarCrash.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Controllers
{
    public class HomeController : Controller
    {
        private InferenceSession _session;

        private ICarCrashRepository repo { get; set; }
        public HomeController(ICarCrashRepository temp)
        {
            repo = temp;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Data(int crashSeverity, int pageNum = 1)
        {
            int pageSize = 10;

            var x = new CrashesViewModel
            {
                Crashes = repo.Crashes.
                OrderBy(p => p.CRASH_DATETIME)
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize),

                PageInfo = new PageInfo
                {
                    TotalNumCrashes = repo.Crashes.Count(),
                    CrashesPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };
           
            List<Location> Locations = repo.Locations.ToList();
            List<Road> Roads = repo.Roads.ToList();
            Dictionary<int, string> locations = new Dictionary<int, string>();
            Dictionary<int, string> roads = new Dictionary<int, string>();

            var Crashes = repo.Crashes.
            OrderBy(p => p.CRASH_DATETIME)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize);

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
            return View(x);
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
