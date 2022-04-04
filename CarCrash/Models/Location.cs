using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public class Location
    {
        [Key]
        [Required]
        public int LOCATION_ID { get; set; }
        public string CITY { get; set; }
        public string COUNTY_NAME { get; set; }
    }
}
