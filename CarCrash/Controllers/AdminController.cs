using CarCrash.Models;
using CarCrash.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ICarCrashRepository repo { get; set; }
        public AdminController (ICarCrashRepository temp)
        {
            repo = temp;
        }

        public IActionResult Index()
        {
            return Redirect("account/login");
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
                        locations[l.LOCATION_ID] = l.CITY + ", " + l.COUNTY_NAME;
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
                repo.Save(c);
                return RedirectToPage("/Admin/Data");
            }
            else
            {
                return View();
            }

        }

        [HttpGet]
        public IActionResult Edit(int crashId)
        {
            ViewBag.crash = repo.Crashes.ToList();
            ViewBag.loc = repo.Locations.ToList();
            ViewBag.road = repo.Roads.ToList();
            var crash = repo.Crashes.Single(x => x.CRASH_ID == crashId);

            return View("Edit", crash);
        }

        [HttpPost]
        public IActionResult Edit(Crash c)
        {
            repo.Update(c);

            return RedirectToAction("Data");
        }

        [HttpGet]
        public IActionResult Delete(int crashid)
        {
            ViewBag.crash = repo.Crashes.ToList();
            var crash = repo.Crashes.Single(x => x.CRASH_ID == crashid);
            return View("Delete", crash);
        }

        [HttpPost]
        public IActionResult Delete(Crash c)
        {
            repo.DeleteIt(c);
            return RedirectToAction("Data");
        }
        
        [HttpGet]
        public IActionResult Locations()
        {
            ViewBag.max = repo.Locations.Select(x => x.LOCATION_ID).Max();
            ViewBag.loc = repo.Locations.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Locations(Location l)
        {
            if (ModelState.IsValid)
            {
                repo.SaveLocation(l);
                return RedirectToAction("Data");
            }
            else
            {
                ViewBag.loc = repo.Locations.ToList();
                return View();
            }
        }

        [HttpGet]
        public IActionResult Roads()
        {
            ViewBag.max = repo.Roads.Select(x => x.ROAD_ID).Max();
            ViewBag.road = repo.Roads.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Roads(Road r)
        {
            if (ModelState.IsValid)
            {
                repo.SaveRoad(r);
                return RedirectToAction("Data");
            }
            else
            {
                ViewBag.road = repo.Locations.ToList();
                return View();
            }
        }
    }
}
