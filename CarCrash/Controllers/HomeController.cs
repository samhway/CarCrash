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
        public HomeController(ICarCrashRepository temp, InferenceSession session)
        {
            repo = temp;
            _session = session;
        }
        [HttpPost]
        public ActionResult Prediction(Crashd data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<float> score = result.First().AsTensor<float>();
            var prediction = new Predictiond { PredictedValue = score.First() };
            result.Dispose();
            ViewBag.Prediction = prediction.PredictedValue;
            ViewBag.Prediction = Math.Round(ViewBag.Prediction);
            return View("Prediction");
        }
        public class Predictiond
        {
            public float PredictedValue { get; set; }
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
        [HttpGet]
        public IActionResult Prediction()
        {
            ViewBag.Prediction = null;
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

        public IActionResult Locations(int LocationId = 0)
        {
            Location Location = repo.Locations.FirstOrDefault(x => x.LOCATION_ID == LocationId);

            return View(Location);
        }

        public IActionResult Roads(int RoadId = 0)
        {
            Road Road = repo.Roads.FirstOrDefault(x => x.ROAD_ID == RoadId);

            return View(Road);
        }
    }
}
