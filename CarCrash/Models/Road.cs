using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public class Road
    {
        [Key]
        [Required]
        public int ROAD_ID { get; set; }
        public string MAIN_ROAD_NAME { get; set; }
    }
}
