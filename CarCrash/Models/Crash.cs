using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public class Crash
    {
        [Key]
        [Required]
        public int CRASH_ID { get; set; }
        public string CRASH_DATETIME { get; set; }
        public int Minute { get; set; }
        public int Hour { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string ?ROUTE { get; set; }
        public decimal ?MILEPOINT { get; set; }
        public decimal ?LAT_UTM_Y { get; set; }
        public decimal ?LONG_UTM_X { get; set; }
        public int ROAD_ID { get; set; }
        public int LOCATION_ID { get; set; }
        public int CRASH_SEVERITY_ID { get; set; }
        public int ?WORK_ZONE_RELATED { get; set; }
        public int ?PEDESTRIAN_INVOLVED { get; set; }
        public int ?BICYCLIST_INVOLVED { get; set; }
        public int ?MOTORCYCLE_INVOLVED { get; set; }
        public int ?IMPROPER_RESTRAINT { get; set; }
        public int ?UNRESTRAINED { get; set; }
        public int ?DUI { get; set; }
        public int ?INTERSECTION_RELATED { get; set; }
        public int ?WILD_ANIMAL_RELATED { get; set; }
        public int ?DOMESTIC_ANIMAL_RELATED { get; set; }
        public int ?OVERTURN_ROLLOVER { get; set; }
        public int ?COMMERCIAL_MOTOR_VEH_INVOLVED { get; set; }
        public int ?TEENAGE_DRIVER_INVOLVED { get; set; }
        public int ?OLDER_DRIVER_INVOLVED { get; set; }
        public int ?NIGHT_DARK_CONDITION { get; set; }
        public int ?SINGLE_VEHICLE { get; set; }
        public int ?DISTRACTED_DRIVING { get; set; }
        public int ?DROWSY_DRIVING { get; set; }
        public int ?ROADWAY_DEPARTURE { get; set; }

    }
}
