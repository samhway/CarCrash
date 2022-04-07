using CarCrash.Models;
using CarCrash.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

        public IActionResult FilterData(bool ?CycInv, bool ?PedInv, bool ?WSRel, bool ?MotInv, bool ?ImpRes, bool ?Unr, bool ?DUI, bool ?IntRel, bool ?AniRel, bool ?DomAniRel, bool ?OveRol, bool ?ComVeh, bool ?TenDri, bool ?OldDri, bool ?Night, bool ?Single, bool ?Dist, bool ?Drows, bool ?Depart,int ?ID, int ?Sev, string ?Date, string ?Loc, int pageNum = 1)
        {
            int pageSize = 10;

            var Crashes = repo.Crashes.
                OrderBy(p => p.CRASH_DATETIME);
            if(PedInv != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.PEDESTRIAN_INVOLVED == PedInv);
            }
            if (CycInv != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.BICYCLIST_INVOLVED == CycInv);
            }
            if (WSRel != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.WORK_ZONE_RELATED == WSRel);
            }
            if (MotInv != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.MOTORCYCLE_INVOLVED == MotInv);
            }
            if (ImpRes != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.IMPROPER_RESTRAINT == ImpRes);
            }
            if (Unr != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.UNRESTRAINED == Unr);
            }
            if (DUI != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.DUI == DUI);
            }
            if (IntRel != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.INTERSECTION_RELATED == IntRel);
            }
            if (AniRel != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.WILD_ANIMAL_RELATED == AniRel);
            }
            if (DomAniRel != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.DOMESTIC_ANIMAL_RELATED == DomAniRel);
            }
            if (OveRol != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.OVERTURN_ROLLOVER == OveRol);
            }
            if (ComVeh != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.COMMERCIAL_MOTOR_VEH_INVOLVED == ComVeh);
            }
            if (TenDri != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.TEENAGE_DRIVER_INVOLVED == TenDri);
            }
            if (OldDri != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.OLDER_DRIVER_INVOLVED == OldDri);
            }
            if (Night != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.NIGHT_DARK_CONDITION == Night);
            }
            if (Single != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.SINGLE_VEHICLE == Single);
            }
            if (Dist != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.DISTRACTED_DRIVING == Dist);
            }
            if (Drows != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.DROWSY_DRIVING == Drows);
            }
            if (Depart != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.ROADWAY_DEPARTURE == Depart);
            }
            if (ID != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.CRASH_ID == ID);
            }
            if (Sev != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.CRASH_SEVERITY_ID == Sev);
            }
            if (Date != null)
            {
                Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.CRASH_DATETIME.Contains(Date));
            }
            //if (Loc != null)
            //{
            //    Crashes = (IOrderedQueryable<Crash>)Crashes.Where(p => p.CRASH_DATETIME.Contains(Loc));
            //}


            var x = new CrashesViewModel
            {
                Crashes = Crashes.Skip((pageNum - 1) * pageSize).Take(pageSize),


                PageInfo = new PageInfo
                {
                    TotalNumCrashes = repo.Crashes.Count(),
                    CrashesPerPage = pageSize,
                    CurrentPage = pageNum
                }
            };
            //var filtered = repo.Crashes.Where(p => p.BICYCLIST_INVOLVED == CycInv & p.PEDESTRIAN_INVOLVED == PedInv & p.WORK_ZONE_RELATED == WSRel).ToList();

            List<Location> Locations = repo.Locations.ToList();
            List<Road> Roads = repo.Roads.ToList();
            Dictionary<int, string> locations = new Dictionary<int, string>();
            Dictionary<int, string> roads = new Dictionary<int, string>();
            Crashes = (IOrderedQueryable<Crash>)Crashes.Skip((pageNum - 1) * pageSize).Take(pageSize);

            var count = Crashes.Count();
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
            return View("Data", x);
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
            var Crashes = repo.Crashes.
            OrderBy(p => p.CRASH_DATETIME)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize);
            var count = Crashes.Count();
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
            return View(x);
        }

        [HttpGet]
        public IActionResult Prediction()
        {
            ViewBag.Prediction = null;
            return View();
        }

        //[HttpPost]
        //public IActionResult Prediction()
        //{
        //    return View();
        //}
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

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
